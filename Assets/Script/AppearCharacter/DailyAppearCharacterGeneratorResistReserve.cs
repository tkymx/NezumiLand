using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public class DailyAppearCharacterGeneratorResistReserve
    {
        private AppearCharacterGenerator appearCharacterGenerator;
        private IDailyAppearCharacterRegistCondition dailyAppearCharacterRegistCondition;

        public DailyAppearCharacterGeneratorResistReserve(AppearCharacterGenerator appearCharacterGenerator, IDailyAppearCharacterRegistCondition dailyAppearCharacterRegistCondition)
        {
            this.appearCharacterGenerator = appearCharacterGenerator;
            this.dailyAppearCharacterRegistCondition = dailyAppearCharacterRegistCondition;
        }

        public bool IsResist() {
            return this.dailyAppearCharacterRegistCondition.IsResist();
        }

        public bool IsRemove() {
            return this.dailyAppearCharacterRegistCondition.IsOnce();
        }

        public void Generate() {
            GameManager.Instance.AppearCharacterManager.EnqueueRegister(this.appearCharacterGenerator.Generate());
        }

        public override string ToString() {
            return this.appearCharacterGenerator.ToString() + " " + this.dailyAppearCharacterRegistCondition.ToString();  
        }

        public bool IsTarget (AppearCharacterViewModel appearCharacterViewModel) {
            return this.appearCharacterGenerator.IsTarget(appearCharacterViewModel);
        }
    }
}