using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public struct ArrangementItemAmount
    {
        private long value;
        public long Value => value;

        public ArrangementItemAmount(long value)
        {
            this.value = value;
        }

        public static ArrangementItemAmount operator +(ArrangementItemAmount left, ArrangementItemAmount right)
        {
            return new ArrangementItemAmount(left.value + right.value);
        }

        public static ArrangementItemAmount operator -(ArrangementItemAmount left, ArrangementItemAmount right)
        {
            return new ArrangementItemAmount(left.value - right.value);
        }

        public static bool operator <(ArrangementItemAmount left, ArrangementItemAmount right)
        {
            return left.value < right.value;
        }

        public static bool operator >(ArrangementItemAmount left, ArrangementItemAmount right)
        {
            return left.value > right.value;
        }

        public static bool operator <=(ArrangementItemAmount left, ArrangementItemAmount right)
        {
            return left.value <= right.value;
        }

        public static bool operator >=(ArrangementItemAmount left, ArrangementItemAmount right)
        {
            return left.value > right.value;
        }

        // 掛け算は存在しない
        public override string ToString()
        {
            return value.ToString() + "個";
        }
    }
}
