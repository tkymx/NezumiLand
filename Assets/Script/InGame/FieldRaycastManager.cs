using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

namespace NL {
    public class FieldRaycastManager {

        /// <summary>
        /// フィールドをタップする時に反応しないオブジェクトマスク
        /// </summary>
        public enum MaskMode {
            None,
            All,
            Field,
            RemoveArrangement,
            MoveArrangement
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
            if (Physics.Raycast (ray, out Hit, Mathf.Infinity, LayerMask.GetMask (GetMask()))) {
                var select = Hit.transform.GetComponent<SelectBase> ();
                Debug.Assert (select != null, "選択したオブジェクトに ISelect がありません");

                if (select != null) {
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

        private string[] GetMask() {
            if (this.maskMode == MaskMode.None) {
                return new string [] {};
            }
            if (this.maskMode == MaskMode.RemoveArrangement) {
                return new string[] { "SelectLayer" };
            }
            if (this.maskMode == MaskMode.MoveArrangement) {
                return new string[] { "SelectLayer", "Floor" };
            }
            if (this.maskMode == MaskMode.Field) {
                return new string[] { "Floor" };
            }
            return new string[] { "SelectLayer", "Floor", "Mouse", "Coin", "Home" };
        }

        private bool CanTouch () {
            return EventSystem.current.currentSelectedGameObject == null;
        }
    }
}