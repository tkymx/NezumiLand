using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NL {
    public class MonoListCellView : ListCellViewBase {
        [SerializeField]
        private Text monoName = null;

        [SerializeField]
        private Text makingFree = null;

        [SerializeField]
        private Text makingAmount = null;

        [SerializeField]
        private Text appearCount = null;

        [SerializeField]
        private Text appearMaxCount = null;

        [SerializeField]
        private Button releaseLock = null;

        [SerializeField]
        private Button detailButton = null;

        private TypeObservable<int> onClickDetail = null;
        public TypeObservable<int> OnClickDetail => onClickDetail;

        private TypeObservable<int> onClickLock = null;
        public TypeObservable<int> OnClickLock => onClickLock;

        public override void Initialize() {
            base.Initialize();
            this.onClickDetail = new TypeObservable<int>();
            this.onClickLock = new TypeObservable<int>();
            this.detailButton.onClick.AddListener(() => {
                this.onClickDetail.Execute(0);
            });
            this.releaseLock.onClick.AddListener(() => {
                this.onClickLock.Execute(0);
            });
        }

        public void UpdateCell (string name, Currency free, ArrangementItemAmount amount, long appearCount, long appearMaxCount, bool isLock) {
            this.monoName.text = name;
            this.makingFree.text = free.ToString ();
            this.makingAmount.text = amount.ToString();
            this.appearCount.text = appearCount.ToString ();
            this.appearMaxCount.text = appearMaxCount.ToString ();
            this.releaseLock.gameObject.SetActive(isLock);
        }
    }
}