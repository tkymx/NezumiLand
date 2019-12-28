using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class CameraMoveManager
    {
        [SerializeField]
        private Transform targetTransform;

        public CameraMoveManager(Transform target)
        {
            this.targetTransform = target;            
        }

        private bool isDragging = false;
        private Vector3 startPosition = Vector3.zero;

        // Update is called once per frame
        public void UpdateByFrame()
        {
            // ドラッグしていないとき
            if (!GameManager.Instance.InputManager.IsDragging) {
                this.isDragging = false;
                return;
            }

            // ドラッグの開始
            if (!this.isDragging) {
                this.isDragging = true;
                this.startPosition = this.targetTransform.position;
            }

            // ドラッグ処理
            if (this.isDragging) {
                this.targetTransform.position = this.startPosition + GameManager.Instance.InputManager.DraggingFromStart;
            }
        }
    }    
}