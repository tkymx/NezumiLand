using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {

    /// <summary>
    /// 報酬のモデル
    /// 受け取りを行うことができる。
    /// </summary>
    public class RewardModel : ModelBase
    {
        public List<IRewardAmount> RewardAmounts { get; private set; }

        public RewardModel(
            uint id,
            List<IRewardAmount> rewardAmounts 
        ) {
            this.Id = id;
            this.RewardAmounts = rewardAmounts;
        }
    }
}