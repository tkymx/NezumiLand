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

        [SerializeField]
        private Text satisfaction = null;

        [SerializeField]
        private Button entryButton = null;

        [SerializeField]
        private Button cancelButton = null;

        private TypeObservable<int> onBack = null;
        public TypeObservable<int> OnBack => onBack;

        private TypeObservable<int> onCancel = null;
        public TypeObservable<int> OnCancel => onCancel;

        private TypeObservable<int> onEntry = null;
        public TypeObservable<int> OnEntry => onEntry;

        public void Initialize() {            
            onBack = new TypeObservable<int>();
            back.onClick.AddListener(() => {
                onBack.Execute(0);
            });

            onCancel = new TypeObservable<int>();
            cancelButton.onClick.AddListener(() => {
                onCancel.Execute(0);
            });

            onEntry = new TypeObservable<int>();
            entryButton.onClick.AddListener(() => {
                onEntry.Execute(0);
            });

            this.SetEntryButtonEnabled(false);
            this.SetCnacelButtonEnabled(false);
        }

        private float closeTime = 0;

        public void UpdateCell (string title, string detail, string author, bool isCloseTIme, float closeTime, string satisfaction) {
            this.titleText.text = title;
            this.detailText.text = detail;
            this.authorText.text = author;
            this.closeTimeView.UpdateView(isCloseTIme, closeTime);
            this.satisfaction.text = "取得満足度: " + satisfaction;
        }

        public void SetEntryButtonEnabled(bool isEnabled)
        {
            this.entryButton.gameObject.SetActive(isEnabled);
        }

        public void SetCnacelButtonEnabled(bool isEnabled)
        {
            this.cancelButton.gameObject.SetActive(isEnabled);
        }
    }
}