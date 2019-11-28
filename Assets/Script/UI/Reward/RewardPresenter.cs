using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public class RewardPresenter : UiWindowPresenterBase
    {
        [SerializeField]
        private RewardItemView rewardItemView = null;

        private IRewardAmount rewardAmount = null;
        public void SetRewardAmount(IRewardAmount rewardAmount) {
            this.rewardAmount = rewardAmount;
        }

        public void Initialize() {
            this.rewardItemView.Initialize();
            this.rewardItemView.OnClickObservable.Subscribe(_ => {
                this.Close();                
            });
            this.Close();
        }

        public override void onPrepareShow() {
            Debug.Assert(rewardAmount != null, "報酬がありません");
            rewardItemView.UpdateReward(this.rewardAmount.Name, this.rewardAmount.Amount, this.rewardAmount.Image);
        }

        public override void onPrepareClose() {
            Debug.Assert(this.rewardAmount != null, "報酬がありません");
            this.rewardAmount = null;
        }
    }
}
