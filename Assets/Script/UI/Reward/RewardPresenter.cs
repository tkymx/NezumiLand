using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public class RewardPresenter : RewardWindowPresenterBase
    {
        [SerializeField]
        private RewardItemView rewardItemView = null;

        private IRewardAmount rewardAmount = null;

        private void Awake() {
            this.rewardAmount = RewardGenerator.InValidReward;
        }

        public override void SetRewardAmount(IRewardAmount rewardAmount) {
            Debug.Assert(rewardAmount != null, "報酬がありません");
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
            Debug.Assert(rewardAmount != null, "報酬がありません");
            this.rewardAmount = null;
        }
    }
}
