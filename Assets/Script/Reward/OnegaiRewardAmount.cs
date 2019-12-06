using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public class OnegaiRewardAmount : IRewardAmount
    {
        private uint onegaiId = 0;
        public uint OnegaiId => onegaiId;

        public OnegaiRewardAmount(uint amount, uint onegaiId)
        {
            this.onegaiId = onegaiId;
            this.Amount = amount;            
            this.Image = ResourceLoader.LoadItemSprite("Onegai");       
        }

        public string Name => "お願い";
        public RewardType RewardType => RewardType.Onegai;
        public uint Amount { get; private set; }
        public Sprite Image { get; private set; }
        public void Receive() {
            GameManager.Instance.OnegaiManager.EnqueueUnLockReserve(this.onegaiId);
        }
    }
}