using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class ArrangementMenuSelectModeContext
    {
        private IArrangementTarget arrangementTarget;
        public IArrangementTarget ArrangementTarget => arrangementTarget;

        public void SetArrangementTarget(IArrangementTarget arrangementTarget)
        {
            this.arrangementTarget = arrangementTarget;
        }
    }
}
