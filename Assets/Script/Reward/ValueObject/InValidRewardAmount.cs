using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public class InvalidRewardAmount : IRewardAmount
    {
        public InvalidRewardAmount()
        {
            this.Amount = 0;
            this.Image = ResourceLoader.LoadItemSprite("None");
        }

        public string Name => "無効なアイテム";
        public RewardType RewardType => RewardType.None;
        public uint Amount { get; private set; }
        public Sprite Image { get; private set; }
        public void Receive() {
            Debug.Assert(false, "InValidReward は受け取れません");
        }
    }
}