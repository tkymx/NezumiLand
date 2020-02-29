using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NL {
    public class ListCellViewBase : MonoBehaviour {
        [SerializeField]
        protected Button cellButton = null;

        public TypeObservable<int> OnClick { get; private set; }

        public virtual void Initialize () {
            OnClick = new TypeObservable<int> ();
            if (cellButton != null) {
                cellButton.onClick.AddListener (() => {
                    OnClick.Execute (0);
                });
            }
        }

        public void Enable () {
            this.cellButton.interactable = true;
        }

        public void Diasble () {
            this.cellButton.interactable = false;
        }
    }
}