using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public abstract class ListPresenterBase<Element, CellView> : DisposableMonoBehaviour
        where CellView : ListCellViewBase 
    {
        [SerializeField]
        private GameObject cellPrefab = null;

        [SerializeField]
        private GameObject cellViewRoot = null;

        protected Dictionary<Element, CellView> displayElementCellDictionary;
        private List<Element> elements;

        public void SetElement (List<Element> elements) {
            this.displayElementCellDictionary = new Dictionary<Element, CellView> ();
            this.elements = elements;
            this.ReLoad ();
        }

        public void ReLoad () {
            // 寄贈の要素を消去
            foreach (Transform child in cellViewRoot.transform) {
                Object.DisAppear (child.gameObject);
            }

            this.displayElementCellDictionary.Clear ();

            // disposable をクリアする
            this.ClearDisposable();

            // 要素の追加を行う
            foreach (var element in this.elements) {
                var instance = Object.Appear2D (cellPrefab, cellViewRoot, Vector2.zero);
                var cellView = instance.GetComponent<CellView> ();
                cellView.Initialize ();
                onReloadCell (element, cellView);
                this.displayElementCellDictionary.Add (element, cellView);
            }
        }

        protected abstract void onReloadCell (Element element, CellView cellView);
    }
}