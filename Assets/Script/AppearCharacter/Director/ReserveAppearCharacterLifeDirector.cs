using System;

namespace  NL
{
    /// <summary>
    /// 予約機能で訪れることになったキャラクターのディレクター
    /// </summary>
    public class ReserveAppearCharacterLifeDirector : IAppearCharacterLifeDirector 
    {
        private PlayerAppearConversationCharacterDirectorModel playerAppearConversationCharacterDirectorModel;

        public ReserveAppearCharacterLifeDirector(PlayerAppearConversationCharacterDirectorModel playerAppearConversationCharacterDirectorModel)
        {
            this.playerAppearConversationCharacterDirectorModel = playerAppearConversationCharacterDirectorModel;
        }

        public void OnInitializeView(AppearCharacterView appearCharacterView)
        {
            appearCharacterView.SetConversationNotifierEnabled(true);
        }

        public IObservable<int> OnTouch()
        {
            var conversationMode =  GameModeGenerator.GenerateConversationMode(this.playerAppearConversationCharacterDirectorModel.AppearConversationCharacterDirectorModel.ConversationModel);
            GameManager.Instance.GameModeManager.EnqueueChangeModeWithHistory(conversationMode);
            return GameManager.Instance.GameModeManager.GetModeEndObservable(conversationMode)
                .SelectMany<int,int>(_ => {
                    if (this.playerAppearConversationCharacterDirectorModel.AppearConversationCharacterDirectorModel.RewardModel == null) {
                        return new ImmediatelyObservable<int>(_);
                    }
                    if (this.playerAppearConversationCharacterDirectorModel.AppearCharactorWithReward.IsReceiveReward) {
                        return new ImmediatelyObservable<int>(_);
                    }
                    if (this.playerAppearConversationCharacterDirectorModel.AppearConversationCharacterDirectorModel.RewardModel.RewardAmounts.Count <= 0) {
                        return new ImmediatelyObservable<int>(_);
                    }                    

                    // 受け取り済みにする
                    GameManager.Instance.AppearCharacterManager.ToReeiveRewards(this.playerAppearConversationCharacterDirectorModel);

                    var rewardMode = GameModeGenerator.GenerateReceiveRewardMode(this.playerAppearConversationCharacterDirectorModel.AppearConversationCharacterDirectorModel.RewardModel);
                    GameManager.Instance.GameModeManager.EnqueueChangeModeWithHistory(rewardMode);
                    return GameManager.Instance.GameModeManager.GetModeEndObservable(rewardMode);
                })
                .Do(_ => {
                    // TODO 終了後に撤退の動作を始める
                    // 予約から消す必要があれば消す
                    if(GameManager.Instance.DailyAppearCharacterRegistManager.IsRemoveReserve(this.playerAppearConversationCharacterDirectorModel.PlayerAppearCharacterReserveModel)) {
                        GameManager.Instance.DailyAppearCharacterRegistManager.RemoveReserve(this.playerAppearConversationCharacterDirectorModel.PlayerAppearCharacterReserveModel); 
                    }
                });
        }
        public void OnCreate()
        {
            // 次の出現を skip する設定
            GameManager.Instance.DailyAppearCharacterRegistManager.SetReserveNextSkippable(this.playerAppearConversationCharacterDirectorModel.PlayerAppearCharacterReserveModel, true);
        }
        public void OnRemove()
        {
            // 次の出現を skip しない設定
            GameManager.Instance.DailyAppearCharacterRegistManager.SetReserveNextSkippable(this.playerAppearConversationCharacterDirectorModel.PlayerAppearCharacterReserveModel, false);            
        }
    }
}