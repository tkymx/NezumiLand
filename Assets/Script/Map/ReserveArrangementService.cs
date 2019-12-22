using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public class ReserveArrangementService
    {
        public static void Execute (IArrangementTarget arrangementTarget, MonoInfo monoInfo) {
            arrangementTarget.MonoInfo = monoInfo;
            ArrangementResourceHelper.ReserveConsume(arrangementTarget.MonoInfo.ArrangementResourceAmount);
            GameManager.Instance.ArrangementManager.AddArrangement(arrangementTarget);
        }
    }
}

