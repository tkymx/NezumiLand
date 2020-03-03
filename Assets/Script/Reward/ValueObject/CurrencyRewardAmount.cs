using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public class CurrencyRewardAmount : IRewardAmount
    {
        public CurrencyRewardAmount(uint amount)
        {
            this.Amount = amount;     
            this.Image = ResourceLoader.LoadItemSprite("Currency");       
        }

        public string Name => "お金";
        public RewardType RewardType => RewardType.Currency;
        public uint Amount { get; private set; }
        public Sprite Image { get; private set; }        
        public void Receive() {
            GameManager.Instance.Wallet.Push(new Currency(this.Amount));
        }
    }
}