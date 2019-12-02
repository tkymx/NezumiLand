using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public interface IDailyAppearCharacterRegistCondition
    {
        bool IsResist();
        bool IsOnce();
    }
}