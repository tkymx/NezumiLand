using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public class UnReserveArrangementService
    {
        public static void Execute (IArrangementTarget arrangementTarget) {
            ArrangementResourceHelper.UnReserveConsume(arrangementTarget.MonoInfo.ArrangementResourceAmount);
            GameManager.Instance.ArrangementManager.RemoveArranement(arrangementTarget);
        }
    }
}

