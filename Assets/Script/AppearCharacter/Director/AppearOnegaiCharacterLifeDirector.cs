using System;
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

        public AppearOnegaiCharacterLifeDirector(PlayerAppearOnegaiCharacterDirectorModel PlayerAppearOnegaiCharacterDirectorModel)
        {
            var scheduleRepository = new ScheduleRepository(ContextMap.DefaultMap);
            var onegaiRepository = new OnegaiRepository(ContextMap.DefaultMap, scheduleRepository);
            this.playerOnegaiRepository = new PlayerOnegaiRepository(onegaiRepository, PlayerContextMap.DefaultMap);
            this.playerAppearOnegaiCharacterDirectorModel = PlayerAppearOnegaiCharacterDirectorModel;
        }

        public void OnInitializeView(AppearCharacterView appearCharacterView)
        {
            appearCharacterView.SetConversationNotifierEnabled(true);
        }

        public IObservable<int> OnTouch()
        {
            if (!this.playerAppearOnegaiCharacterDirectorModel.AppearCharactorWithReward.IsReceiveReward) 
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
                    var beforeConversationMode = GameModeGenerator.GenerateConversationMode(this.playerAppearOnegaiCharacterDirectorModel.AppearOnegaiCharacterDirectorModel.BeforeConversationModel);
                    GameManager.Instance.GameModeManager.EnqueueChangeModeWithHistory(beforeConversationMode);
                    return GameManager.Instance.GameModeManager.GetModeEndObservable(beforeConversationMode)
                        .SelectMany(_ => {
                            // お願い詳細を見る
                            GameManager.Instance.GameUIManager.OnegaiDetailPresenter.SetOnegaiDetail(playerOnegaiModel);
                            GameManager.Instance.GameUIManager.OnegaiDetailPresenter.Show();
                            return GameManager.Instance.GameUIManager.OnegaiDetailPresenter.OnClose;
                        });
                }
            }

            {
                //// 受け取り後だったら

                // 終了会話をする    
                var afterConversationMode = GameModeGenerator.GenerateConversationMode(this.playerAppearOnegaiCharacterDirectorModel.AppearOnegaiCharacterDirectorModel.AfterConversationModel);
                GameManager.Instance.GameModeManager.EnqueueChangeModeWithHistory(afterConversationMode);
                return GameManager.Instance.GameModeManager.GetModeEndObservable(afterConversationMode); 
            }       
        }
        public void OnCreate()
        {
            // クリア状況の確認
            var playerOnegaiModel = this.playerOnegaiRepository.GetById(this.playerAppearOnegaiCharacterDirectorModel.AppearOnegaiCharacterDirectorModel.OnegaiModel.Id);
            Debug.Assert(!playerOnegaiModel.IsClear(), "お願いをクリアしているのにキャラクタが出現しました");

            // お願いを UnLock する
            GameManager.Instance.OnegaiManager.EnqueueUnLockReserve(playerOnegaiModel.OnegaiModel.Id);

            // TODO :  クリアしているかを確認する？

            // 次の出現を skip する設定
            GameManager.Instance.DailyAppearCharacterRegistManager.SetReserveNextSkippable(this.playerAppearOnegaiCharacterDirectorModel.PlayerAppearCharacterReserveModel, true);
        }
        public void OnRemove()
        {
            var playerOnegaiModel = this.playerOnegaiRepository.GetById(this.playerAppearOnegaiCharacterDirectorModel.AppearOnegaiCharacterDirectorModel.OnegaiModel.Id);

            // お願いがクリアではない場合はLock する
            if ( !playerOnegaiModel.IsClear() ) 
            {
                GameManager.Instance.OnegaiManager.EnqueueUnLockReserve(playerOnegaiModel.OnegaiModel.Id);
            }

            // 次の出現を skip しない設定
            GameManager.Instance.DailyAppearCharacterRegistManager.SetReserveNextSkippable(this.playerAppearOnegaiCharacterDirectorModel.PlayerAppearCharacterReserveModel, false);            
        }
    }
}