using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class PlayerAppearCharacterReserveModel : ModelBase
    {
        public AppearCharacterLifeDirectorType AppearCharacterLifeDirectorType { get; private set; }
        public AppearCharacterDirectorModelBase AppearCharacterDirectorModelBase { get; private set; }
        public IDailyAppearCharacterRegistCondition DailyAppearCharacterRegistCondition { get; private set; }
        public bool IsNextRemove { get; private set; }
        public bool IsNextSkip { get; private set; }

        public PlayerAppearCharacterReserveModel(
            uint id,
            AppearCharacterLifeDirectorType appearCharacterLifeDirectorType,
            AppearCharacterDirectorModelBase appearCharacterDirectorModelBase,
            IDailyAppearCharacterRegistCondition dailyAppearCharacterRegistCondition,
            bool isNextRemove,
            bool isNextSkip)
        {
            this.Id = id;
            this.AppearCharacterLifeDirectorType = appearCharacterLifeDirectorType;
            this.AppearCharacterDirectorModelBase = appearCharacterDirectorModelBase;
            this.DailyAppearCharacterRegistCondition = dailyAppearCharacterRegistCondition;
            this.IsNextRemove = isNextRemove;
            this.IsNextSkip = isNextSkip;
        }

        public void NextRemove() {
            this.IsNextRemove = true;
        }

        public void SetNextSkippable(bool isNextSkip) {
            this.IsNextSkip = isNextSkip;
        }

    }   
}