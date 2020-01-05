using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public class DailyEarnCalculater {
        public static float SatisfactionMultiRate = 1.1f;

        private readonly IPlayerOnegaiRepository playerOnegaiRepository;

        public DailyEarnCalculater (IPlayerOnegaiRepository playerOnegaiRepository) {
            this.playerOnegaiRepository = playerOnegaiRepository;
        }

        /// <summary>
        /// 満足度から稼ぎを計算する
        /// </summary>
        /// <returns>稼ぎ</returns>
        public Currency CalcEarnFromSatisfaction () {
            var satisfactionCalculater = new SatisfactionCalculater (playerOnegaiRepository);
            Satisfaction currentSatisfaction = satisfactionCalculater.CalcFieldSatisfaction ();
            Currency currency = new Currency ((long)(currentSatisfaction.Value * DailyEarnCalculater.SatisfactionMultiRate));
            return currency;
        }
    }
}