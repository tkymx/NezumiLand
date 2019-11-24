using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public struct ArrangementResourceAmount {
        private Currency currency;
        public Currency Currency => currency;

        private ArrangementItemAmount arrangementItemAmount;
        public ArrangementItemAmount ArrangementItemAmount => arrangementItemAmount;

        private ArrangementCount arrangementCount;
        public ArrangementCount ArrangementCount => arrangementCount;

        public ArrangementResourceAmount (Currency currency, ArrangementItemAmount arrangementItemAmount, ArrangementCount arrangementCount) {
            this.currency = currency;
            this.arrangementItemAmount = arrangementItemAmount;
            this.arrangementCount = arrangementCount;
        }

        public override string ToString () {
            return string.Format ("({0},{1})", currency.ToString (), arrangementItemAmount.ToString ());
        }
    }
}