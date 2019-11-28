using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public abstract class UiWindowPresenterBase : MonoBehaviour {
        
        [SerializeField]
        private GameObject mainWindowGameObject = null;

        private TypeObservable<int> onShow = new TypeObservable<int>();
        public TypeObservable<int> OnShow => onShow;

        private TypeObservable<int> onClose = new TypeObservable<int>();
        public TypeObservable<int> OnClose => onClose;

        public virtual void onPrepareShow () { }
        public void Show () {
            this.onPrepareShow ();
            this.mainWindowGameObject.SetActive (true);
            this.onShow.Execute(0);
        }

        public virtual void onPrepareClose () { }
        public void Close () {
            this.onPrepareClose ();
            this.mainWindowGameObject.SetActive (false);
            this.onClose.Execute(0);
        }
    }
}