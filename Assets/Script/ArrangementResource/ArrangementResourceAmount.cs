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

        private MouseOrderAmount mouseOrderAmount;

        public MouseOrderAmount MouseOrderAmount => mouseOrderAmount;

        public ArrangementResourceAmount (Currency currency, ArrangementItemAmount arrangementItemAmount, ArrangementCount arrangementCount, MouseOrderAmount mouseOrderAmount) {
            this.currency = currency;
            this.arrangementItemAmount = arrangementItemAmount;
            this.arrangementCount = arrangementCount;
            this.mouseOrderAmount = mouseOrderAmount;
        }

        public static ArrangementResourceAmount Zero {
            get {
                return new ArrangementResourceAmount(
                    new Currency(0), 
                    new ArrangementItemAmount(0), 
                    ArrangementCount.Zero,
                    new MouseOrderAmount(0));
            }
        }

        public static ArrangementResourceAmount operator + (ArrangementResourceAmount left, ArrangementResourceAmount right) {
            return new ArrangementResourceAmount (
                    left.currency + right.currency, 
                    left.arrangementItemAmount + right.arrangementItemAmount, 
                    left.arrangementCount + right.arrangementCount,
                    left.mouseOrderAmount + right.mouseOrderAmount);
        }

        public static ArrangementResourceAmount operator - (ArrangementResourceAmount left, ArrangementResourceAmount right) {
            return new ArrangementResourceAmount (
                    left.currency - right.currency, 
                    left.arrangementItemAmount - right.arrangementItemAmount, 
                    left.arrangementCount - right.arrangementCount,
                    left.mouseOrderAmount - right.mouseOrderAmount);
        }        

        public override string ToString () {
            return string.Format ("({0},{1})", currency.ToString (), arrangementItemAmount.ToString ());
        }
    }
}