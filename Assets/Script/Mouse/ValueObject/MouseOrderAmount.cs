using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public struct MouseOrderAmount : IConsumableAmount{
        private long value;
        public long Value => value;

        public MouseOrderAmount (long value) {
            this.value = value;
        }

        public IConsumableAmount Add_Implementation(IConsumableAmount right) {
            Debug.Assert(right is MouseOrderAmount, "MouseOrderAmount ではありません");
            var rightCast = (MouseOrderAmount)right;
            return new MouseOrderAmount(this.value + rightCast.value);
        }

        public IConsumableAmount Subtraction_Implementation(IConsumableAmount right) {
            Debug.Assert(right is MouseOrderAmount, "MouseOrderAmount ではありません");
            var rightCast = (MouseOrderAmount)right;
            return new MouseOrderAmount(this.value - rightCast.value);
        }

        public static MouseOrderAmount operator + (MouseOrderAmount left, MouseOrderAmount right) {
            return (MouseOrderAmount)left.Add_Implementation(right);
        }

        public static MouseOrderAmount operator - (MouseOrderAmount left, MouseOrderAmount right) {
            return (MouseOrderAmount)left.Subtraction_Implementation(right);
        }        

        public static MouseOrderAmount operator ++ (MouseOrderAmount left) {
            return new MouseOrderAmount (left.value + 1);
        }

        public static bool operator < (MouseOrderAmount left, MouseOrderAmount right) {
            return left.value < right.value;
        }

        public static bool operator > (MouseOrderAmount left, MouseOrderAmount right) {
            return left.value > right.value;
        }

        public static bool operator <= (MouseOrderAmount left, MouseOrderAmount right) {
            return left.value <= right.value;
        }

        public static bool operator >= (MouseOrderAmount left, MouseOrderAmount right) {
            return left.value >= right.value;
        }

        public static MouseOrderAmount One {
            get {
                return new MouseOrderAmount(1);
            }
        }

        public override string ToString() {
            return value.ToString();
        }
    }
}