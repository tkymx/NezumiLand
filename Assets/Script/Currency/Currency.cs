using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public struct Currency {
        private long value;
        public long Value => value;

        public Currency (long value) {
            this.value = value;
        }

        public static Currency operator + (Currency left, Currency right) {
            return new Currency (left.value + right.value);
        }

        public static Currency operator - (Currency left, Currency right) {
            return new Currency (left.value - right.value);
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
            return left.value > right.value;
        }

        // 掛け算は存在しない
        public override string ToString () {
            return value.ToString () + "yen";
        }
    }
}