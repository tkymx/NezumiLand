using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public class MonoRewardAmount : IRewardAmount
    {
        private uint monoInfoId = 0;
        public uint MonoInfoId => monoInfoId;

        public MonoRewardAmount(uint amount, uint monoId)
        {
            this.monoInfoId = monoId;
            this.Amount = amount;            
            this.Image = ResourceLoader.LoadItemSprite("Mono");       
        }

        public string Name => "モノ";
        public RewardType RewardType => RewardType.Mono;
        public uint Amount { get; private set; }
        public Sprite Image { get; private set; }
        public void Receive() {
            GameManager.Instance.MonoReleaseManager.EnqueueReserveReleaseMonoId(monoInfoId);
        }
    }
}