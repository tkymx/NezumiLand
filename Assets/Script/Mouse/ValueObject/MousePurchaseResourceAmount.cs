using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public struct MousePurchaseResourceAmount {
        private Currency currency;
        public Currency Currency => currency;

        private ArrangementItemAmount arrangementItemAmount;
        public ArrangementItemAmount ArrangementItemAmount => arrangementItemAmount;

        public MousePurchaseResourceAmount (Currency currency, ArrangementItemAmount arrangementItemAmount) {
            this.currency = currency;
            this.arrangementItemAmount = arrangementItemAmount;
        }

        public override string ToString () {
            return string.Format ("({0},{1})", currency.ToString (), arrangementItemAmount.ToString ());
        }
    }
}