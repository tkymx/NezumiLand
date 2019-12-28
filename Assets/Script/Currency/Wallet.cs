using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {

    public abstract class ConsumableCollectionBase<Amount> 
        where Amount : IConsumableAmount
    {
        public abstract Amount Current { get; }

        public abstract Amount CurrentWithReserve { get; }

        public bool IsConsume (Amount amount = default(Amount), bool withReserve = true) {
            if (withReserve) {
                Amount reserveAmount = GameManager.Instance.ReserveAmountManager.Get<Amount>();
                amount = (Amount)amount.Add_Implementation(reserveAmount);
            }
            var result = OnIsConsume(amount);
            return result;
        }

        public abstract bool OnIsConsume (Amount amount);

        public void Consume (Amount amount) {
            Debug.Assert(IsConsume(amount, false), "消費することができません" + typeof(Amount).GetType().ToString());
            OnConsume(amount);
        }

        public abstract void OnConsume (Amount amount);
    }

    public class Wallet : ConsumableCollectionBase<Currency>{

        private UpdateCurrencyService updateCurrencyService = null;

        private Currency current;
        public override Currency Current => current;
        public override Currency CurrentWithReserve => current - GameManager.Instance.ReserveAmountManager.Get<Currency>();

        public Wallet (Currency currency, IPlayerInfoRepository playerInfoRepository) {
            this.current = currency;
            this.updateCurrencyService = new UpdateCurrencyService(playerInfoRepository);
        }

        public void ForceSet (Currency currency) {
            this.current = currency;
        }

        public override bool OnIsConsume(Currency fee) {
            var result = current >= fee;
            return result;
        }

        public override void OnConsume (Currency fee) {
            Debug.Assert (OnIsConsume (fee), "払うことができません");
            current -= fee;
            this.updateCurrencyService.Execute(current);
        }

        public void Push (Currency fee) {
            current += fee;
            this.updateCurrencyService.Execute(current);
        }
    }
}