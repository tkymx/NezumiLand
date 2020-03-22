using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class CameraMoveManager
    {
        // 極座標系
        // 左手座標系を考慮
        private struct PolarCoordinate
        {
            public float R;
            public float Theta;
            public float Phai;

            public void From(Vector3 position)
            {
                this.R = position.magnitude;
                Debug.Assert(this.R > 1e-8, "値が小さすぎます" + position.ToString());

                this.Theta = Mathf.Asin( position.y / this.R);
                this.Phai = Mathf.Atan2( position.z, position.x);
            }

            public Vector3 To()
            {
                var pos = new Vector3();
                pos.x = this.R * Mathf.Cos(this.Theta) * Mathf.Cos(this.Phai);
                pos.y = this.R * Mathf.Sin(this.Theta);
                pos.z = this.R * Mathf.Cos(this.Theta) * Mathf.Sin(this.Phai);
                return pos;
            }
        }


        [SerializeField]
        private Transform targetTransform;

        [SerializeField]
        private Transform targetCenterTransform;

        private PolarCoordinate polarCoordinate;

        public CameraMoveManager(Transform target, Transform targetCenter)
        {
            this.targetTransform = target;
            this.targetCenterTransform = targetCenter;

            this.polarCoordinate = new PolarCoordinate();
            this.polarCoordinate.From(targetTransform.localPosition);
        }

        private bool isDragging = false;
        private float startTheta = 0;
        private float startPahi = 0;

        private bool isPinching = false;
        private float startR = 0;

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
                this.startPahi = this.polarCoordinate.Phai;
                this.startTheta = this.polarCoordinate.Theta;
            }

            // ドラッグ処理
            if (this.isDragging) {
                var phaiAmount = GameManager.Instance.InputManager.DraggingFromStart.x * GameManager.Instance.GlobalSystemParameter.RotationMoveFactor;
                var thetaAmount = GameManager.Instance.InputManager.DraggingFromStart.y * GameManager.Instance.GlobalSystemParameter.RotationMoveFactor;

                this.polarCoordinate.Phai = this.startPahi + phaiAmount;
                this.polarCoordinate.Theta = this.startTheta + thetaAmount;

                // 上限と下限
                this.polarCoordinate.Theta = Mathf.Clamp(this.polarCoordinate.Theta, Mathf.Deg2Rad * 10, Mathf.Deg2Rad * 80);

                this.UpdateCoordinate();
            }
        }

        private void UpdatePinch()
        {
            if (!GameManager.Instance.InputManager.IsPinching) {
                this.isPinching = false;
                return;
            }

            if (!this.isPinching) {
                this.isPinching = true;
                this.startR = this.polarCoordinate.R;
            }

            if (this.isPinching) {
                var moveAmount = GameManager.Instance.InputManager.PinchingFromStart  * GameManager.Instance.GlobalSystemParameter.PinchMoveFactor;
                this.polarCoordinate.R = startR + moveAmount;

                // 上限と下限
                this.polarCoordinate.R = Mathf.Clamp(this.polarCoordinate.R, GameManager.Instance.GlobalSystemParameter.PinchNearLength, GameManager.Instance.GlobalSystemParameter.PinchFarLength);

                this.UpdateCoordinate();
            }
        }

        private void UpdateCoordinate()
        {
            this.targetTransform.localPosition = this.polarCoordinate.To();
            this.targetTransform.transform.LookAt(new Vector3());
        }
    }    
}