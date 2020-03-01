using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    /// <summary>
    /// 開放の報酬情報
    /// </summary>
    public struct ParkOpenRewardAmount
    {
        public int ObtainHeartCount { get; private set; }
        public RewardModel Reward { get; private set; }

        public ParkOpenRewardAmount(
            int obtainHeartCount,
            RewardModel reward
        )
        {
            this.ObtainHeartCount = obtainHeartCount;
            this.Reward = reward;
        }
    }
}

