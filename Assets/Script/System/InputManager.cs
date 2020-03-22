using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class InputManager
    {
        private const float movableDragMagnitude = 9;
        private const float moveFactor = 0.05f;

        enum State {
            Idle,
            PreMove,
            Move,
            Pinch
        };

        private State currentState;
        private Vector3 startMousePosition;
        private Vector3[] startPichPositions = new Vector3[2];

        /// <summary>
        /// 現在シングルタップしているか？
        /// ※ １フレームのみ有効
        /// </summary>
        private bool isSingleTap = false;
        public bool IsSingleTap {
            get {
                return isSingleTap;
            }
        }

        /// <summary>
        /// 現在ドラッグ中かどうか？
        /// ※ １フレームのみ有効
        /// </summary>
        private bool isDragging = false;
        public bool IsDragging {
            get {
                return isDragging;
            }
        }

        /// <summary>
        /// ドラッグしている場合、スタート地点からどれくらいドラッグしているか？
        /// </summary>
        Vector3 draggingFromStart = Vector3.zero;
        public Vector3 DraggingFromStart {
            get {
                return draggingFromStart;
            }
        }

        /// <summary>
        /// ピンチしているかどうか？
        /// </summary>
        private bool isPinching = false;
        public bool IsPinching {
            get {
                return this.isPinching;
            }
        }

        /// <summary>
        /// ピンチしているときのスタートからの距離
        /// </summary>
        float pinchingFromStart = 0.0f;
        public float PinchingFromStart {
            get {
                return pinchingFromStart;
            }
        }

        public void Initialize () {
            this.currentState = State.Idle;
        }

        public void UpdateByFrame () {

            this.isSingleTap = false;
            this.isDragging = false;
            this.isPinching = false;
            this.draggingFromStart = Vector3.zero;
            this.pinchingFromStart = 0;

            if (!GameManager.Instance.GameModeManager.IsCameraMobableMode) {
                this.currentState = State.Idle;
                return;
            }

            // 指に本以上ならピンチ
            if (Input.touchCount >= 2) {
                if (this.currentState != State.Pinch)
                {
                    this.currentState = State.Pinch;
                    this.startPichPositions[0] = Input.touches[0].position;
                    this.startPichPositions[1] = Input.touches[1].position;
                }
            }

            switch (this.currentState) {
                case State.Idle: {
                    if (Input.GetMouseButtonDown(0)) {
                        this.currentState = State.PreMove;
                        this.startMousePosition = Input.mousePosition;
                    }
                    break;
                }
                case State.PreMove: {
                    var delta = Input.mousePosition - this.startMousePosition;
                    if (delta.magnitude > movableDragMagnitude) {
                        this.currentState = State.Move;
                    }
                    if (Input.GetMouseButtonUp(0)) {
                        this.currentState = State.Idle;
                        this.isSingleTap = true;
                    }
                    break;
                }
                case State.Move: {
                    this.isDragging = true;

                    var delta = Input.mousePosition - this.startMousePosition;
                    this.draggingFromStart = new Vector3(-delta.x * moveFactor, -delta.y * moveFactor, 0);
                    if (Input.GetMouseButtonUp(0)) {
                        this.currentState = State.Idle;
                    }
                    break;
                }
                case State.Pinch: {
                    if (Input.touchCount < 2) {
                        this.currentState = State.Idle;
                        break;
                    }

                    this.isPinching = true;

                    // 幅の変更
                    var startDistance = (this.startPichPositions[0] - this.startPichPositions[1]).magnitude;
                    var currentDistance = (Input.touches[0].position - Input.touches[1].position).magnitude;
                    this.pinchingFromStart = currentDistance - startDistance;

                    break;
                }
                default: {
                    this.currentState = State.Idle;
                    break;
                }
            }
        }
    }   
}