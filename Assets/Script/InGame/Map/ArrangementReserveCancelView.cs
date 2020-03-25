using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NL
{
    public class ArrangementReserveCancelView : MonoBehaviour
    {
        private readonly float DefaultSize = 100;

        public enum Direction
        {
            Left,
            Right,
            Up,
            Down
        }

        [SerializeField]
        private Button onCancel = null;

        [SerializeField]
        private Button onLeft = null;

        [SerializeField]
        private Button onRight = null;

        [SerializeField]
        private Button onUp = null;

        [SerializeField]
        private Button onDown = null;

        [SerializeField]
        private Canvas mainCanvas = null;

        public TypeObservable<int> OnCancelObservable { get; private set; } 

        public TypeObservable<Direction> OnClickDirection { get; private set; }

        public void Initialize (Camera mainCamera) {
            this.mainCanvas.worldCamera = mainCamera;

            this.OnCancelObservable = new TypeObservable<int>();
            this.OnClickDirection = new TypeObservable<Direction>();

            this.onCancel.onClick.AddListener(() => {
                this.OnCancelObservable.Execute(0);
            });
            this.onLeft.onClick.AddListener(() => {
                this.OnClickDirection.Execute(Direction.Left);
            });
            this.onRight.onClick.AddListener(() => {
                this.OnClickDirection.Execute(Direction.Right);
            });
            this.onUp.onClick.AddListener(() => {
                this.OnClickDirection.Execute(Direction.Up);
            });
            this.onDown.onClick.AddListener(() => {
                this.OnClickDirection.Execute(Direction.Down);
            });
        }

        public void SetAnnnotationSize(int factorX, int factorY)
        {
            var rect = this.mainCanvas.GetComponent<RectTransform>();
            Debug.Assert(rect != null, "RectTransform が取得できません。");
            rect.sizeDelta = new Vector2(factorX, factorY) * DefaultSize;
        }
    }   
}
