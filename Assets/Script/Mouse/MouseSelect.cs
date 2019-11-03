using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class MouseSelect : SelectBase
    {
        public override void OnOver(RaycastHit hit)
        {

        }
        public override void OnSelect(RaycastHit hit)
        {
            GameManager.Instance.GameModeManager.EnqueueChangeMode(GameModeGenerator.GenerateMenuSelectMode());
        }
    }
}