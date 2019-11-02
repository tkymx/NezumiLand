using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class MenuSelectModeContext
    {
        private MonoInfo selecetedMonoInfo = null;
        public MonoInfo SelectedMonoInfo => selecetedMonoInfo;

        public bool HasMonoInfo => selecetedMonoInfo != null;

        public void SelectMonoInfo(MonoInfo monoInfo)
        {
            this.selecetedMonoInfo = monoInfo;
        }

        public void RemoveSelectMonoInfo()
        {
            this.selecetedMonoInfo = null;
        }
    }
}

