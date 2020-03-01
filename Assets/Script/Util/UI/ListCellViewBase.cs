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
            if (cellButton != null) {
                this.cellButton.interactable = true;
            }
        }

        public void Diasble () {
            if (cellButton != null) {
                this.cellButton.interactable = false;
            }
        }
    }
}