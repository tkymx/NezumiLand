using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class InputManager
    {
        private const float movableDragMagnitude = 3;
        private const float moveFactor = 0.1f;

        enum State {
            Idle,
            PreMove,
            Move
        };

        private State currentState;
        private Vector3 startMousePosition;

        private bool isSingleTap = false;
        public bool IsSingleTap {
            get {
                return isSingleTap;
            }
        }

        private bool isDragging = false;
        public bool IsDragging {
            get {
                return isDragging;
            }
        }

        Vector3 draggingFromStart = Vector3.zero;
        public Vector3 DraggingFromStart {
            get {
                return draggingFromStart;
            }
        }

        public void Initialize () {
            this.currentState = State.Idle;
        }

        public void UpdateByFrame () {

            this.isSingleTap = false;
            this.isDragging = false;
            this.draggingFromStart = Vector3.zero;

            if (!GameManager.Instance.GameModeManager.IsCameraMobableMode) {
                this.currentState = State.Idle;
                return;
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
                    this.draggingFromStart = new Vector3(-delta.x * moveFactor, 0, -delta.y * moveFactor);
                    if (Input.GetMouseButtonUp(0)) {
                        this.currentState = State.Idle;
                    }
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