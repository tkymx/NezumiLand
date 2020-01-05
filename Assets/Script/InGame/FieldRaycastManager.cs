using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

namespace NL {
    public class FieldRaycastManager {

        public enum MaskMode {
            All,
            Field,
        }

        private Camera mainCamera;

        private MaskMode maskMode;
        public void SetMaskMode (MaskMode maskMode) {
            this.maskMode = maskMode;
        }

        public FieldRaycastManager (Camera mainCamera) {
            this.mainCamera = mainCamera;
            this.maskMode = MaskMode.All;
        }

        public void UpdateByFrame () {
            GameManager.Instance.ArrangementManager.ArrangementAnnotater.RemoveAllAnnotation ();

            var ray = mainCamera.ScreenPointToRay (Input.mousePosition);

            RaycastHit Hit;
            if (Physics.Raycast (ray, out Hit, Mathf.Infinity, LayerMask.GetMask (new string[] { "SelectLayer", "Floor", "Mouse" }))) {
                var select = Hit.transform.GetComponent<SelectBase> ();
                Debug.Assert (select != null, "選択したオブジェクトに ISelect がありません");

                if (select != null) {
                    if (GetMask().Any(mask => mask == LayerMask.LayerToName(select.gameObject.layer))) {
                        if (GameManager.Instance.InputManager.IsSingleTap) {
                            if (CanTouch ()) {
                                select.OnSelect (Hit);
                            }
                        } else {
                            select.OnOver (Hit);
                        }
                    }
                }
            }
        }

        private string[] GetMask() {
            if (this.maskMode == MaskMode.Field) {
                return new string[] { "Floor" };
            }
            return new string[] { "SelectLayer", "Floor", "Mouse" };
        }

        private bool CanTouch () {
            return EventSystem.current.currentSelectedGameObject == null;
        }
    }
}