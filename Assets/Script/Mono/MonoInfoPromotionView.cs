using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NL
{
    public class MonoInfoPromotionView : MonoBehaviour
    {
        [SerializeField]
        private Button obtainButton;

        [SerializeField]
        private Text promotionCountText;

        [SerializeField]
        private Canvas mainCanvas;

        public TypeObservable<int> OnTouchObservable { get; private set; }

        public void Initialize(Camera mainCamera)
        {
            this.mainCanvas.worldCamera = mainCamera;

            this.OnTouchObservable = new TypeObservable<int>();
            this.obtainButton.onClick.AddListener(() => {
                this.OnTouchObservable.Execute(0);
            });
        }

        public void UpdateView(string promotionCount)
        {
            this.promotionCountText.text = promotionCount;
        }
    }
}