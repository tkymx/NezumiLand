using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class ArrangementSelect : SelectBase
    {
        [SerializeField]
        private ArrangementView arrangementView = null;

        public override void OnOver(RaycastHit hit)
        {

        }

        public override void OnSelect(RaycastHit hit)
        {
            arrangementView.OnSelect.Execute(0);
        }
    }
}