using System;
using System.Collections.Generic;
using UnityEngine;

namespace  NL
{
    /// <summary>
    /// 予約機能で訪れることになったお願い持ちキャラクターのディレクター
    /// </summary>
    public class AppearOnegaiCharacterLifeDirector : IAppearCharacterLifeDirector 
    {
        private readonly IPlayerOnegaiRepository playerOnegaiRepository;
        private readonly PlayerAppearOnegaiCharacterDirectorModel playerAppearOnegaiCharacterDirectorModel;

        private AppearCharacterView appearCharacterView = null;

        public AppearOnegaiCharacterLifeDirector(PlayerAppearOnegaiCharacterDirectorModel PlayerAppearOnegaiCharacterDirectorModel)
        {
            var scheduleRepository = new ScheduleRepository(ContextMap.DefaultMap);
            var onegaiRepository = new OnegaiRepository(ContextMap.DefaultMap, scheduleRepository);
            this.playerOnegaiRepository = new PlayerOnegaiRepository(onegaiRepository, PlayerContextMap.DefaultMap);
            this.playerAppearOnegaiCharacterDirectorModel = PlayerAppearOnegaiCharacterDirectorModel;
        }

        public void OnInitializeView(AppearCharacterView appearCharacterView)
        {
            this.appearCharacterView = appearCharacterView;
            this.appearCharacterView.SetConversationNotifierEnabled(false);
        }

        public IObservable<int> OnTouch()
        {
            // おねがいの状態によって会話を変える。
            var playerOnegaiModel = this.playerOnegaiRepository.GetById(this.playerAppearOnegaiCharacterDirectorModel.AppearOnegaiCharacterDirectorModel.OnegaiModel.Id);
            if (playerOnegaiModel.IsClear())
            {
                /// clear だったら
                /// クリア時の会話を実行して、次からは出てこないようにする

                var afterConversationMode = GameModeGenerator.GenerateConversationMode(this.playerAppearOnegaiCharacterDirectorModel.AppearOnegaiCharacterDirectorModel.AfterConversationModel);
                GameManager.Instance.GameModeManager.EnqueueChangeModeWithHistory(afterConversationMode);
                return GameManager.Instance.GameModeManager.GetModeEndObservable(afterConversationMode)
                    .Do(_ => {
                        // 予約から消す必要があれば消す
                        if(GameManager.Instance.DailyAppearCharacterRegistManager.IsRemoveReserve(this.playerAppearOnegaiCharacterDirectorModel.PlayerAppearCharacterReserveModel)) {
                            GameManager.Instance.DailyAppearCharacterRegistManager.RemoveReserve(this.playerAppearOnegaiCharacterDirectorModel.PlayerAppearCharacterReserveModel); 
                        }
                    });
            }
            else if (playerOnegaiModel.IsUnLock())
            {
                /// お願いを受け取っていなかったら
                /// 受け取り中の会話を実行して詳細を開く
                
                var middleConversationMode = GameModeGenerator.GenerateConversationMode(this.playerAppearOnegaiCharacterDirectorModel.AppearOnegaiCharacterDirectorModel.MiddleConversationModel);
                GameManager.Instance.GameModeManager.EnqueueChangeModeWithHistory(middleConversationMode);
                return GameManager.Instance.GameModeManager.GetModeEndObservable(middleConversationMode)
                    .SelectMany(_ => OpenOnegaiProcess());
            }
            else if (playerOnegaiModel.IsLock())
            {
                /// お願いを受け取っていなかったら
                /// 受け取り前の会話を実行して詳細を開く
                
                var beforeConversationMode = GameModeGenerator.GenerateConversationMode(this.playerAppearOnegaiCharacterDirectorModel.AppearOnegaiCharacterDirectorModel.BeforeConversationModel);
                GameManager.Instance.GameModeManager.EnqueueChangeModeWithHistory(beforeConversationMode);
                return GameManager.Instance.GameModeManager.GetModeEndObservable(beforeConversationMode)
                    .SelectMany(_ => OpenOnegaiProcess());
            }

            Debug.LogError("不定な状態です " + playerOnegaiModel.OnegaiState.ToString());
            return null;
        }

        private IObservable<int> OpenOnegaiProcess()
        {
            var playerOnegaiModel = this.playerOnegaiRepository.GetById(this.playerAppearOnegaiCharacterDirectorModel.AppearOnegaiCharacterDirectorModel.OnegaiModel.Id);
            if (playerOnegaiModel.IsLock())
            {
                GameManager.Instance.GameUIManager.OnegaiDetailPresenter.SetOnegaiDetailWithEntry(playerOnegaiModel);
            }
            else
            {
                GameManager.Instance.GameUIManager.OnegaiDetailPresenter.SetOnegaiDetailWithCancel(playerOnegaiModel);
            }
            GameManager.Instance.GameUIManager.OnegaiDetailPresenter.Show();

            // 次に進む用のオブザーバブルを作成する
            List<IDisposable> disposables = new List<IDisposable>();
            TypeObservable<int> subject = new TypeObservable<int>();
            Action doNext = () => {
                foreach(var disposable in disposables) {
                    disposable.Dispose();
                }
                subject.Execute(0);
            };

            // close ボタン
            disposables.Add(GameManager.Instance.GameUIManager.OnegaiDetailPresenter.OnClose.Subscribe(__ => {
                doNext();
            }));

            // entryButton
            disposables.Add(GameManager.Instance.GameUIManager.OnegaiDetailPresenter.OnEntry.Subscribe(__ => {

                // お願いを受け取る
                Debug.Assert(playerOnegaiModel.IsLock(), "すでに開放されています。" + playerOnegaiModel.OnegaiModel.Id.ToString());
                GameManager.Instance.OnegaiManager.EnqueueUnLockReserve(playerOnegaiModel.OnegaiModel.Id);

                // 受取時 の詳細を表示
                GameManager.Instance.GameUIManager.CommonPresenter.SetContents("お願いを受け取りました", playerOnegaiModel.OnegaiModel.Title);
                GameManager.Instance.GameUIManager.CommonPresenter.Show();
                disposables.Add(GameManager.Instance.GameUIManager.CommonPresenter.OnClose.Subscribe(___ => {
                    GameManager.Instance.GameUIManager.OnegaiDetailPresenter.Close();
                    doNext();
                }));
            }));            

            // cancelButton
            disposables.Add(GameManager.Instance.GameUIManager.OnegaiDetailPresenter.OnCancel.Subscribe(__ => {

                // お願いが開放されていない状態にする
                Debug.Assert(playerOnegaiModel.IsUnLock(), "UnLockではない状態です" + playerOnegaiModel.OnegaiModel.Id.ToString());
                GameManager.Instance.OnegaiManager.EnqueueLockReserve(playerOnegaiModel.OnegaiModel.Id);

                // cancel の詳細を表示
                GameManager.Instance.GameUIManager.CommonPresenter.SetContents("お願いを断りました", playerOnegaiModel.OnegaiModel.Title);
                GameManager.Instance.GameUIManager.CommonPresenter.Show();
                disposables.Add(GameManager.Instance.GameUIManager.CommonPresenter.OnClose.Subscribe(___ => {
                    GameManager.Instance.GameUIManager.OnegaiDetailPresenter.Close();
                    doNext();
                }));
            }));            

            return subject;
        }

        public void OnCreate()
        {
            // 次の出現を skip する設定
            GameManager.Instance.DailyAppearCharacterRegistManager.SetReserveNextSkippable(this.playerAppearOnegaiCharacterDirectorModel.PlayerAppearCharacterReserveModel, true);
        }

        public void OnUpdateByFrame()
        {
            var playerOnegaiModel = this.playerOnegaiRepository.GetById(this.playerAppearOnegaiCharacterDirectorModel.AppearOnegaiCharacterDirectorModel.OnegaiModel.Id);
            this.appearCharacterView.EnableOnegaiIndicator(playerOnegaiModel.OnegaiState);
        }

        public void OnRemove()
        {
            var playerOnegaiModel = this.playerOnegaiRepository.GetById(this.playerAppearOnegaiCharacterDirectorModel.AppearOnegaiCharacterDirectorModel.OnegaiModel.Id);

            // お願いがクリアではない場合はLock する
            if ( !playerOnegaiModel.IsClear() ) 
            {
                GameManager.Instance.OnegaiManager.EnqueueLockReserve(playerOnegaiModel.OnegaiModel.Id);
            }

            // 次の出現を skip しない設定
            GameManager.Instance.DailyAppearCharacterRegistManager.SetReserveNextSkippable(this.playerAppearOnegaiCharacterDirectorModel.PlayerAppearCharacterReserveModel, false);            
        }
    }
}