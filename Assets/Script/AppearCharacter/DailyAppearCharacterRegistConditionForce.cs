﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public class DailyAppearCharacterRegistConditionForce : IDailyAppearCharacterRegistCondition
    {
        public bool IsOnce() {
            return true;
        }
        public bool IsResist() {
            return true;
        }
    }
}