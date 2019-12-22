using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public class ArrangementItemStore : ConsumableCollectionBase<ArrangementItemAmount>{
        private ArrangementItemAmount current;
        public override ArrangementItemAmount Current => current;
        public override ArrangementItemAmount CurrentWithReserve => current - GameManager.Instance.ReserveAmountManager.Get<ArrangementItemAmount>();

        public ArrangementItemStore (ArrangementItemAmount ArrangementItemAmount) {
            this.current = ArrangementItemAmount;
        }

        public override bool OnIsConsume (ArrangementItemAmount amount) {
            return this.current >= amount;
        }

        public override void OnConsume (ArrangementItemAmount amount) {
            Debug.Assert (IsConsume (amount), "消費することができません");
            this.current -= amount;
        }

        public void Push (ArrangementItemAmount amount) {
            this.current += amount;
        }
    }
}