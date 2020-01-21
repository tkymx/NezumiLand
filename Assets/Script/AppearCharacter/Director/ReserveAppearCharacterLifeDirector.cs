using System;

namespace  NL
{
    /// <summary>
    /// 予約機能で訪れることになったキャラクターのディレクター
    /// </summary>
    public class ReserveAppearCharacterLifeDirector : IAppearCharacterLifeDirector 
    {
        private PlayerAppearCharacterViewModel playerAppearCharacterViewModel;
        private PlayerAppearCharacterReserveModel playerAppearCharacterReserveModel;

        public ReserveAppearCharacterLifeDirector(PlayerAppearCharacterViewModel playerAppearCharacterViewModel, PlayerAppearCharacterReserveModel playerAppearCharacterReserveModel)
        {
            this.playerAppearCharacterViewModel = playerAppearCharacterViewModel;
            this.playerAppearCharacterReserveModel = playerAppearCharacterReserveModel;
        }

        public IObservable<int> OnTouch()
        {
            var conversationMode =  GameModeGenerator.GenerateConversationMode(this.playerAppearCharacterReserveModel.ConversationModel);
            GameManager.Instance.GameModeManager.EnqueueChangeModeWithHistory(conversationMode);
            return GameManager.Instance.GameModeManager.GetModeEndObservable(conversationMode)
                .SelectMany(_ => {
                    if (this.playerAppearCharacterReserveModel.RewardModel == null) {
                        return new ImmediatelyObservable<int>(_);
                    }
                    if (this.playerAppearCharacterViewModel.IsReceiveReward) {
                        return new ImmediatelyObservable<int>(_);
                    }
                    if (this.playerAppearCharacterReserveModel.RewardModel.RewardAmounts.Count <= 0) {
                        return new ImmediatelyObservable<int>(_);
                    }                    

                    // 受け取り済みにする
                    GameManager.Instance.AppearCharacterManager.ToReeiveRewards(this.playerAppearCharacterViewModel);

                    var rewardMode = GameModeGenerator.GenerateReceiveRewardMode(this.playerAppearCharacterReserveModel.RewardModel);
                    GameManager.Instance.GameModeManager.EnqueueChangeModeWithHistory(rewardMode);
                    return GameManager.Instance.GameModeManager.GetModeEndObservable(rewardMode);
                })
                .Do(_ => {
                    // TODO 終了後に撤退の動作を始める
                    // 予約から消す必要があれば消す
                    if(GameManager.Instance.DailyAppearCharacterRegistManager.IsRemoveReserve(this.playerAppearCharacterReserveModel)) {
                        GameManager.Instance.DailyAppearCharacterRegistManager.RemoveReserve(this.playerAppearCharacterReserveModel); 
                    }
                });
        }
        public void OnCreate()
        {
            // 次の出現を skip する設定
            GameManager.Instance.DailyAppearCharacterRegistManager.SetReserveNextSkippable(this.playerAppearCharacterReserveModel, true);
        }
        public void OnRemove()
        {
            // 次の出現を skip しない設定
            GameManager.Instance.DailyAppearCharacterRegistManager.SetReserveNextSkippable(this.playerAppearCharacterReserveModel, false);            
        }
    }
}