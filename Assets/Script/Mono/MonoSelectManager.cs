using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class MonoSelectManager
    {
        private MonoInfo selectedMonoInfo;
        public MonoInfo SelectedMonoInfo => selectedMonoInfo;
        public bool HasSelectedMonoInfo => selectedMonoInfo != null;

        public void SelectMonoInfo(MonoInfo MonoInfo)
        {
            this.selectedMonoInfo = MonoInfo;
        }

        public void RemoveSelect()
        {
            this.selectedMonoInfo = null;
        }
    }
}

