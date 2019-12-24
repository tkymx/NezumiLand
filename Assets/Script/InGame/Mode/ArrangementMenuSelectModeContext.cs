using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public class ArrangementMenuSelectModeContext {
        private IPlayerArrangementTarget arrangementTarget;
        public IPlayerArrangementTarget ArrangementTarget => arrangementTarget;

        public void SetArrangementTarget (IPlayerArrangementTarget arrangementTarget) {
            this.arrangementTarget = arrangementTarget;
        }
    }
}