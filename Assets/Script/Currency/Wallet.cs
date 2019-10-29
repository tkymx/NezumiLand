using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class Wallet
    {
        private Currency currentCurrency;
        public Currency CurrentCurrency => currentCurrency;

        public Wallet(Currency currency)
        {
            this.currentCurrency = currency;
        }

        public bool IsPay(Currency fee)
        {
            return currentCurrency.Value >= fee.Value;
        }

        public void Pay(Currency fee)
        {
            Debug.Assert(IsPay(fee), "払うことができません");
            currentCurrency -= fee;
        }

        public void Push(Currency fee)
        {
            currentCurrency += fee;
        }
    }
}
