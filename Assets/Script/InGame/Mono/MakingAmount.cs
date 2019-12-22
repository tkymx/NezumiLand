using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public struct MakingAmount {

        private float value;
        public float Value => value;

        private float maxValue;
        public float MaxValue => maxValue;

        public float Rate => value / maxValue;

        public bool IsFinish => Rate >= 1.0f;

        public MakingAmount (float value, float maxValue) {
            this.value = value;
            this.maxValue = maxValue;
        }        

        public static MakingAmount operator + (MakingAmount left, MakingAmount right) {
            return new MakingAmount (left.value + right.value, Mathf.Max(left.maxValue,right.maxValue));
        }

        public static MakingAmount operator - (MakingAmount left, MakingAmount right) {
            return new MakingAmount (left.value - right.value, Mathf.Max(left.maxValue,right.maxValue));
        }

        public static bool operator < (MakingAmount left, MakingAmount right) {
            return left.value < right.value;
        }

        public static bool operator > (MakingAmount left, MakingAmount right) {
            return left.value > right.value;
        }

        public static bool operator <= (MakingAmount left, MakingAmount right) {
            return left.value <= right.value;
        }

        public static bool operator >= (MakingAmount left, MakingAmount right) {
            return left.value >= right.value;
        }
    }
}