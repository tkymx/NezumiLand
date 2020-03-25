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
            if (this.playerAppearOnegaiCharacterDirectorModel.IsCancel)
            {
                // 残念会話
                var cancelConversationMode = GameModeGenerator.GenerateConversationMode(this.playerAppearOnegaiCharacterDirectorModel.AppearOnegaiCharacterDirectorModel.CancelConversationModel);
                GameManager.Instance.GameModeManager.EnqueueChangeModeWithHistory(cancelConversationMode);
                return GameManager.Instance.GameModeManager.GetModeEndObservable(cancelConversationMode);
            }
            else if (!this.playerAppearOnegaiCharacterDirectorModel.AppearCharactorWithReward.IsReceiveReward) 
            {
                //// 受け取り前だったら
                
                // クリア状況の確認
                var playerOnegaiModel = this.playerOnegaiRepository.GetById(this.playerAppearOnegaiCharacterDirectorModel.AppearOnegaiCharacterDirectorModel.OnegaiModel.Id);
                if (playerOnegaiModel.IsClear())
                {
                    //// clear だったら

                    // 会話をする
                    var afterConversationMode = GameModeGenerator.GenerateConversationMode(this.playerAppearOnegaiCharacterDirectorModel.AppearOnegaiCharacterDirectorModel.AfterConversationModel);
                    GameManager.Instance.GameModeManager.EnqueueChangeModeWithHistory(afterConversationMode);
                    return GameManager.Instance.GameModeManager.GetModeEndObservable(afterConversationMode)
                        .Do(_ => {
                            // 予約から消す必要があれば消す
                            if(GameManager.Instance.DailyAppearCharacterRegistManager.IsRemoveReserve(this.playerAppearOnegaiCharacterDirectorModel.PlayerAppearCharacterReserveModel)) {
                                GameManager.Instance.DailyAppearCharacterRegistManager.RemoveReserve(this.playerAppearOnegaiCharacterDirectorModel.PlayerAppearCharacterReserveModel); 
                            }
                        })
                        .SelectMany(_ => {
                            // 受け取り済みにする
                            GameManager.Instance.AppearCharacterManager.ToReeiveRewards(this.playerAppearOnegaiCharacterDirectorModel);
                            // 報酬を受け取る                            
                            var rewardMode = GameModeGenerator.GenerateReceiveRewardMode(this.playerAppearOnegaiCharacterDirectorModel.AppearOnegaiCharacterDirectorModel.RewardModel);
                            GameManager.Instance.GameModeManager.EnqueueChangeModeWithHistory(rewardMode);
                            return GameManager.Instance.GameModeManager.GetModeEndObservable(rewardMode);
                        });
                }
                else
                {
                    //// clear じゃなかったら
                    
                    // 会話する
                    return OpenOnegaiProcess(playerOnegaiModel);
                }
            }
            else
            {
                //// 受け取り後だったら
                
                var playerOnegaiModel = this.playerOnegaiRepository.GetById(this.playerAppearOnegaiCharacterDirectorModel.AppearOnegaiCharacterDirectorModel.OnegaiModel.Id);
                if (playerOnegaiModel.IsClear())
                {
                    ///　クリアだったら
                    var afterConversationMode = GameModeGenerator.GenerateConversationMode(this.playerAppearOnegaiCharacterDirectorModel.AppearOnegaiCharacterDirectorModel.AfterConversationModel);
                    GameManager.Instance.GameModeManager.EnqueueChangeModeWithHistory(afterConversationMode);
                    return GameManager.Instance.GameModeManager.GetModeEndObservable(afterConversationMode); 
                }
                else
                {
                    return OpenOnegaiProcess(playerOnegaiModel);
                }
            }
        }

        private IObservable<int> OpenOnegaiProcess(PlayerOnegaiModel playerOnegaiModel)
        {
            var beforeConversationMode = GameModeGenerator.GenerateConversationMode(this.playerAppearOnegaiCharacterDirectorModel.AppearOnegaiCharacterDirectorModel.BeforeConversationModel);
            GameManager.Instance.GameModeManager.EnqueueChangeModeWithHistory(beforeConversationMode);
            return GameManager.Instance.GameModeManager.GetModeEndObservable(beforeConversationMode)
                .SelectMany(_ => {
                    // お願い詳細を見る
                    GameManager.Instance.GameUIManager.OnegaiDetailPresenter.SetOnegaiDetail(playerOnegaiModel);
                    GameManager.Instance.GameUIManager.OnegaiDetailPresenter.Show();
                    GameManager.Instance.GameUIManager.OnegaiDetailPresenter.SetCnacelButtonEnabled(true);

                    List<IDisposable> disposables = new List<IDisposable>();
                    TypeObservable<int> subject = new TypeObservable<int>();
                    Action doNext = () => {
                        // 次に進む
                        foreach(var disposable in disposables) {
                            disposable.Dispose();
                        }
                        subject.Execute(0);
                    };

                    // close ボタン
                    disposables.Add(GameManager.Instance.GameUIManager.OnegaiDetailPresenter.OnClose.Subscribe(__ => {

                        if (this.playerAppearOnegaiCharacterDirectorModel.IsCancel)
                        {
                            // 残念会話
                            var cancelConversationMode = GameModeGenerator.GenerateConversationMode(this.playerAppearOnegaiCharacterDirectorModel.AppearOnegaiCharacterDirectorModel.CancelConversationModel);
                            GameManager.Instance.GameModeManager.EnqueueChangeModeWithHistory(cancelConversationMode);
                            disposables.Add(GameManager.Instance.GameModeManager.GetModeEndObservable(cancelConversationMode).Subscribe(___ => {
                                doNext();
                            }));
                        }
                        else
                        {
                            doNext();
                        }
                    }));

                    // cancelButton
                    disposables.Add(GameManager.Instance.GameUIManager.OnegaiDetailPresenter.OnCancel.Subscribe(__ => {
                        GameManager.Instance.AppearCharacterManager.Cancel(this.playerAppearOnegaiCharacterDirectorModel);

                        // cancel の詳細を表示
                        GameManager.Instance.GameUIManager.CommonPresenter.SetContents("お願いを断りました", playerOnegaiModel.OnegaiModel.Title);
                        GameManager.Instance.GameUIManager.CommonPresenter.Show();
                        disposables.Add(GameManager.Instance.GameUIManager.CommonPresenter.OnClose.Subscribe(___ => {
                            GameManager.Instance.GameUIManager.OnegaiDetailPresenter.Close();
                        }));
                    }));            

                    return subject;
                });
        }

        public void OnCreate()
        {
            // クリア状況の確認
            var playerOnegaiModel = this.playerOnegaiRepository.GetById(this.playerAppearOnegaiCharacterDirectorModel.AppearOnegaiCharacterDirectorModel.OnegaiModel.Id);

            // お願いを clear していなかったら UnLock する
            if (!playerOnegaiModel.IsClear() && !this.playerAppearOnegaiCharacterDirectorModel.IsCancel )
            {
                GameManager.Instance.OnegaiManager.EnqueueUnLockReserve(playerOnegaiModel.OnegaiModel.Id);
            }

            // TODO: すでにクリアしているかを判定する必要がある。。。

            // 次の出現を skip する設定
            GameManager.Instance.DailyAppearCharacterRegistManager.SetReserveNextSkippable(this.playerAppearOnegaiCharacterDirectorModel.PlayerAppearCharacterReserveModel, true);
        }

        public void OnUpdateByFrame()
        {
            var playerOnegaiModel = this.playerOnegaiRepository.GetById(this.playerAppearOnegaiCharacterDirectorModel.AppearOnegaiCharacterDirectorModel.OnegaiModel.Id);

            // キャンセル時はロックする
            if (this.playerAppearOnegaiCharacterDirectorModel.IsCancel && !playerOnegaiModel.IsLock()) {
                GameManager.Instance.OnegaiManager.EnqueueLockReserve(playerOnegaiModel.OnegaiModel.Id);
            }

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