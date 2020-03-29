using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NL
{
    public class ArrangementMoveView : MonoBehaviour
    {
        private readonly float DefaultSize = 100;

        [SerializeField]
        private Button onCancel = null;

        [SerializeField]
        private Button onOk = null;

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

        public TypeObservable<int> OnOKObservable { get; private set; } 

        public TypeObservable<ArrangementManager.Direction> OnClickDirection { get; private set; }

        public void Initialize (Camera mainCamera) {
            this.mainCanvas.worldCamera = mainCamera;

            this.OnCancelObservable = new TypeObservable<int>();
            this.OnOKObservable = new TypeObservable<int>();
            this.OnClickDirection = new TypeObservable<ArrangementManager.Direction>();

            this.onCancel.onClick.AddListener(() => {
                this.OnCancelObservable.Execute(0);
            });
            this.onOk.onClick.AddListener(() => {
                this.OnOKObservable.Execute(0);
            });
            this.onLeft.onClick.AddListener(() => {
                this.OnClickDirection.Execute(ArrangementManager.Direction.Left);
            });
            this.onRight.onClick.AddListener(() => {
                this.OnClickDirection.Execute(ArrangementManager.Direction.Right);
            });
            this.onUp.onClick.AddListener(() => {
                this.OnClickDirection.Execute(ArrangementManager.Direction.Up);
            });
            this.onDown.onClick.AddListener(() => {
                this.OnClickDirection.Execute(ArrangementManager.Direction.Down);
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
