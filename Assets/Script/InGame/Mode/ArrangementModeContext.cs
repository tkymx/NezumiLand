using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class ArrangementModeContext
    {
        private MonoInfo targetMonoInfo;
        public MonoInfo TargetMonoInfo => targetMonoInfo;

        public void SetTagetMonoInfo(MonoInfo monoInfo)
        {
            this.targetMonoInfo = monoInfo;
        }
    }
}
