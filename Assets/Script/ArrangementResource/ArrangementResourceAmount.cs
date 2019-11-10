using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public struct ArrangementResourceAmount
    {
        private Currency currency;
        public Currency Currency => currency;

        private ArrangementItemAmount arrangementItemAmount;
        public ArrangementItemAmount ArrangementItemAmount => arrangementItemAmount;

        public ArrangementResourceAmount(Currency currency, ArrangementItemAmount arrangementItemAmount)
        {
            this.currency = currency;
            this.arrangementItemAmount = arrangementItemAmount;
        }

        public static ArrangementResourceAmount operator +(ArrangementResourceAmount left, ArrangementResourceAmount right)
        {
            return new ArrangementResourceAmount(left.currency + right.currency, left.arrangementItemAmount + right.arrangementItemAmount);
        }

        public static ArrangementResourceAmount operator -(ArrangementResourceAmount left, ArrangementResourceAmount right)
        {
            return new ArrangementResourceAmount(left.currency - right.currency, left.arrangementItemAmount - right.arrangementItemAmount);
        }

        public override string ToString()
        {
            return string.Format("({0},{1})" ,currency.ToString(), arrangementItemAmount.ToString());
        }
    }
}

