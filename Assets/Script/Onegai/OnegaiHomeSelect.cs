using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public class OnegaiHomeSelect : SelectBase {
        public override void OnOver (RaycastHit hit) { }

        public override void OnSelect (RaycastHit hit) {
            // 別で読んだほうがいいが、、とりあえず            
            GameManager.Instance.GameModeManager.EnqueueChangeMode (GameModeGenerator.GenerateOnegaiSelectMode ());
        }
    }
}