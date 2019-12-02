using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public class DailyAppearCharacterRegistConditionByChance : IDailyAppearCharacterRegistCondition
    {
        private float rate = 0;

        public DailyAppearCharacterRegistConditionByChance(float rate)
        {
            this.rate = rate;            
        }
        
        public bool IsOnce() {
            return false;
        }

        public bool IsResist() {
            var currentRate = Random.Range(0.0f, 1.0f);
            return currentRate < this.rate;
        }
    }
}