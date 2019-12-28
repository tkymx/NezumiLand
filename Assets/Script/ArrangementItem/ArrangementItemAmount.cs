using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public struct ArrangementItemAmount : IConsumableAmount {
        private long value;
        public long Value => value;

        public ArrangementItemAmount (long value) {
            this.value = value;
        }

        public IConsumableAmount Add_Implementation(IConsumableAmount right) {
            Debug.Assert(right is ArrangementItemAmount, "ArrangementItemAmount ではありません");
            var rightCast = (ArrangementItemAmount)right;
            return new ArrangementItemAmount(this.value + rightCast.value);
        }

        public IConsumableAmount Subtraction_Implementation(IConsumableAmount right) {
            Debug.Assert(right is ArrangementItemAmount, "ArrangementItemAmount ではありません");
            var rightCast = (ArrangementItemAmount)right;
            return new ArrangementItemAmount(this.value - rightCast.value);
        }

        public static ArrangementItemAmount operator + (ArrangementItemAmount left, ArrangementItemAmount right) {
            return (ArrangementItemAmount)left.Add_Implementation(right);
        }

        public static ArrangementItemAmount operator - (ArrangementItemAmount left, ArrangementItemAmount right) {
            return (ArrangementItemAmount)left.Subtraction_Implementation(right);
        }

        public static bool operator < (ArrangementItemAmount left, ArrangementItemAmount right) {
            return left.value < right.value;
        }

        public static bool operator > (ArrangementItemAmount left, ArrangementItemAmount right) {
            return left.value > right.value;
        }

        public static bool operator <= (ArrangementItemAmount left, ArrangementItemAmount right) {
            return left.value <= right.value;
        }

        public static bool operator >= (ArrangementItemAmount left, ArrangementItemAmount right) {
            return left.value >= right.value;
        }

        // 掛け算は存在しない
        public override string ToString () {
            return value.ToString ();
        }
    }
}