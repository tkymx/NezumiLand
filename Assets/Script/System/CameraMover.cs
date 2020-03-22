using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class CameraMoveManager
    {
        [SerializeField]
        private Transform targetTransform;

        [SerializeField]
        private Transform targetCenterTransform;

        public CameraMoveManager(Transform target, Transform targetCenter)
        {
            this.targetTransform = target;
            this.targetCenterTransform = targetCenter;
        }

        private bool isDragging = false;
        private Vector3 startRotation = Vector3.zero;

        private bool isPinching = false;
        private Vector3 startPosition = Vector3.zero;

        // Update is called once per frame
        public void UpdateByFrame()
        {
            UpdateDrag();
            UpdatePinch();
        }

        private void UpdateDrag()
        {
            // ドラッグしていないとき
            if (!GameManager.Instance.InputManager.IsDragging) {
                this.isDragging = false;
                return;
            }

            // ドラッグの開始
            if (!this.isDragging) {
                this.isDragging = true;
                this.startRotation = this.targetCenterTransform.eulerAngles;
            }

            // ドラッグ処理
            if (this.isDragging) {
                this.targetCenterTransform.eulerAngles =  startRotation - new Vector3(0,GameManager.Instance.InputManager.DraggingFromStart.x,0);
            }
        }

        private void UpdatePinch()
        {
            if (!GameManager.Instance.InputManager.IsPinching) {
                this.isPinching = false;
            }

            if (!this.isPinching) {
                this.isPinching = true;
                this.startPosition = this.targetTransform.localPosition;
            }

            if (this.isPinching) {
                var vec = (this.targetCenterTransform.localPosition - this.targetTransform.localPosition).normalized;
                this.targetTransform.localPosition = this.startPosition + vec * GameManager.Instance.InputManager.PinchingFromStart;
            }
        }
    }    
}