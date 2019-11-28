using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public class ItemRewardAmount : IRewardAmount
    {
        public ItemRewardAmount(uint amount)
        {
            this.Amount = amount;            
            this.Image = ResourceLoader.LoadItemSprite("Item");       
        }

        public string Name => "素材アイテム";
        public RewardType RewardType => RewardType.Item;
        public uint Amount { get; private set; }
        public Sprite Image { get; private set; }
        public void Receive() {
            GameManager.Instance.ArrangementItemStore.Push(new ArrangementItemAmount(this.Amount));
        }
    }
}