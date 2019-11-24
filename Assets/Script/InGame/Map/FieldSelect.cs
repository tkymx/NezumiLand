using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public class FieldSelect : SelectBase {
        public override void OnOver (RaycastHit hit) {
            if (!GameManager.Instance.ArrangementManager.IsEnable) {
                return;
            }

            var makingMono = GameManager.Instance.MonoSelectManager.SelectedMonoInfo;
            GameManager.Instance.ArrangementManager.ArrangementAnnotater.Annotate (ArrangementInfoGenerator.Generate (hit.point, makingMono));
        }

        public override void OnSelect (RaycastHit hit) {
            if (!GameManager.Instance.ArrangementManager.IsEnable) {
                return;
            }

            var makingMono = GameManager.Instance.MonoSelectManager.SelectedMonoInfo;

            GameManager.Instance.ArrangementManager.ArrangementAnnotater.Select ();
            GameManager.Instance.ArrangementManager.ArrangementAnnotater.Annotate (ArrangementInfoGenerator.Generate (hit.point, makingMono));
        }
    }
}