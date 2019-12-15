using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NL {
    public class OnegaiDetailView : MonoBehaviour {
        [SerializeField]
        private Text titleText = null;

        [SerializeField]
        private Text detailText = null;

        [SerializeField]
        private Text authorText = null;

        [SerializeField]
        private OnegaiCloseTimeView closeTimeView = null;

        [SerializeField]
        private Button back = null;

        private TypeObservable<int> onBack = null;
        public TypeObservable<int> OnBack => onBack;

        public void Initialize() {
            onBack = new TypeObservable<int>();
            back.onClick.AddListener(() => {
                onBack.Execute(0);
            });
        }

        private float closeTime = 0;

        public void UpdateCell (string title, string detail, string author, bool isCloseTIme, float closeTime) {
            this.titleText.text = title;
            this.detailText.text = detail;
            this.authorText.text = author;
            this.closeTimeView.UpdateView(isCloseTIme, closeTime);
        }
    }
}