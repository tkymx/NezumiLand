using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public struct Satisfaction {
        private long value;
        public long Value => value;

        public Satisfaction (long value) {
            this.value = value;
        }

        public static Satisfaction operator + (Satisfaction left, Satisfaction right) {
            return new Satisfaction (left.value + right.value);
        }

        public static Satisfaction operator - (Satisfaction left, Satisfaction right) {
            return new Satisfaction (left.value - right.value);
        }

        public static bool operator < (Satisfaction left, Satisfaction right) {
            return left.value < right.value;
        }

        public static bool operator > (Satisfaction left, Satisfaction right) {
            return left.value > right.value;
        }

        public static bool operator <= (Satisfaction left, Satisfaction right) {
            return left.value <= right.value;
        }

        public static bool operator >= (Satisfaction left, Satisfaction right) {
            return left.value >= right.value;
        }

        // 掛け算は存在しない
        public override string ToString () {
            return value.ToString ();
        }
    }
}