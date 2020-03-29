using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public class FieldSelect : SelectBase {
        public override void OnOver (RaycastHit hit) {
        }

        public override void OnSelect (RaycastHit hit) {
            if (GameManager.Instance.MonoSelectManager.HasSelectedMonoInfo) {
                var makingMono = GameManager.Instance.MonoSelectManager.SelectedMonoInfo;
                GameManager.Instance.ArrangementManager.ArrangementAnnotater.Annotate (ArrangementInfoGenerator.Generate (hit.point, makingMono));
            }
            GameManager.Instance.ArrangementManager.ArrangementAnnotater.Select ();
        }
    }
}