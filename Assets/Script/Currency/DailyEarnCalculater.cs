using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class DailyEarnCalculater
    {
        /// <summary>
        /// 満足度から稼ぎを計算する
        /// </summary>
        /// <returns>稼ぎ</returns>
        public static Currency CalcEarnFromSatisfaction()
        {
            Satisfaction currentSatisfaction = SatisfactionCalculater.CalcFieldSatisfaction();
            Currency currency = new Currency(currentSatisfaction.Value * 10);
            return currency;
        }
    }
}

