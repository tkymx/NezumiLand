using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public class ArrangementItemStore {
        private ArrangementItemAmount currentArrangementItemAmount;
        public ArrangementItemAmount CurrentArrangementItemAmount => currentArrangementItemAmount;

        public ArrangementItemStore (ArrangementItemAmount ArrangementItemAmount) {
            this.currentArrangementItemAmount = ArrangementItemAmount;
        }

        public bool IsConsume (ArrangementItemAmount amount) {
            return currentArrangementItemAmount.Value >= amount.Value;
        }

        public void Consume (ArrangementItemAmount amount) {
            Debug.Assert (IsConsume (amount), "消費することができません");
            currentArrangementItemAmount -= amount;
        }

        public void Push (ArrangementItemAmount amount) {
            currentArrangementItemAmount += amount;
        }
    }
}