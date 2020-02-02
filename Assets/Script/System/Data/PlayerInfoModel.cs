using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class PlayerInfoModel : ModelBase
    {
        public float ElapsedTime { get; private set; }
        public Currency Currency{ get; private set; }
        public ArrangementItemAmount ArrangementItemAmount { get; private set; }

        public PlayerInfoModel (uint id, float elapsedTime, Currency currency, ArrangementItemAmount arrangementItemAmount) {
            this.Id = id;
            this.ElapsedTime = elapsedTime;
            this.Currency = currency;
            this.ArrangementItemAmount = arrangementItemAmount;
        }

        public void UpdateTime (float elaspedTime) {
            this.ElapsedTime = elaspedTime;
        }

        public void UpdateCurrency (Currency currency) {
            this.Currency = currency;
        }

        public void UpdateArrangementItemAmount(ArrangementItemAmount arrangementItemAmount) {
            this.ArrangementItemAmount = arrangementItemAmount;
        }
    }
}
