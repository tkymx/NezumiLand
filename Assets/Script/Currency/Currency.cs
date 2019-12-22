using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {

    public struct Currency : IConsumableAmount{
        private long value;
        public long Value => value;

        public Currency (long value) {
            this.value = value;
        }

        public IConsumableAmount Add_Implementation(IConsumableAmount right) {
            Debug.Assert(right is Currency, "Currency ではありません");
            var rightCast = (Currency)right;
            return new Currency(this.value + rightCast.value);
        }

        public IConsumableAmount Subtraction_Implementation(IConsumableAmount right) {
            Debug.Assert(right is Currency, "Currency ではありません");
            var rightCast = (Currency)right;
            return new Currency(this.value - rightCast.value);
        }

        public static Currency operator + (Currency left, Currency right) {
            return (Currency)left.Add_Implementation(right);
        }

        public static Currency operator - (Currency left, Currency right) {
            return (Currency)left.Subtraction_Implementation(right);
        }

        public static bool operator < (Currency left, Currency right) {
            return left.value < right.value;
        }

        public static bool operator > (Currency left, Currency right) {
            return left.value > right.value;
        }

        public static bool operator <= (Currency left, Currency right) {
            return left.value <= right.value;
        }

        public static bool operator >= (Currency left, Currency right) {
            return left.value >= right.value;
        }

        // 掛け算は存在しない
        public override string ToString () {
            return value.ToString () + "yen";
        }
    }
}