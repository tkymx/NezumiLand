using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public class ParkOpenGroupRewardAmount : IRewardAmount
    {
        private uint parkOpenGroupId = 0;
        public uint ParkOpenGroupId => parkOpenGroupId;

        public ParkOpenGroupRewardAmount(uint amount, uint parkOpenGroupId)
        {
            this.parkOpenGroupId = parkOpenGroupId;
            this.Amount = amount;            
            this.Image = ResourceLoader.LoadItemSprite("ParkOpenGroup");       
        }

        public string Name => "開放グループ";
        public RewardType RewardType => RewardType.ParkOpenGroup;
        public uint Amount { get; private set; }
        public Sprite Image { get; private set; }
        public void Receive() {
            GameManager.Instance.ParkOpenGroupManager.ToOpen(this.parkOpenGroupId);
        }
    }
}