using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public abstract class UiWindowPresenterBase : DisposableMonoBehaviour {
        
        [SerializeField]
        private GameObject mainWindowGameObject = null;

        private TypeObservable<int> onShow = null;
        public TypeObservable<int> OnShow {
            get {
                if (this.onShow == null) {
                    this.onShow = new TypeObservable<int>();
                }
                return this.onShow;
            }
        }

        private TypeObservable<int> onClose = null;
        public TypeObservable<int> OnClose {
            get {
                if (this.onClose == null) {
                    this.onClose = new TypeObservable<int>();
                }
                return this.onClose;
            }
        }
        public virtual void onPrepareShow () { }
        public void Show () {
            this.onPrepareShow ();
            this.mainWindowGameObject.SetActive (true);
            this.OnShow.Execute(0);
        }

        public virtual void onPrepareClose () { }
        public void Close () {
            this.onPrepareClose ();
            this.mainWindowGameObject.SetActive (false);
            this.OnClose.Execute(0);
        }

        public bool IsShow () {
            return mainWindowGameObject.activeSelf;
        }
    }
}