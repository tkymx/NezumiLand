using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public abstract class UiWindowPresenterBase : MonoBehaviour {
        [SerializeField]
        private GameObject mainWindowGameObject = null;

        public virtual void onPrepareShow () { }
        public void Show () {
            this.onPrepareShow ();
            this.mainWindowGameObject.SetActive (true);
        }

        public virtual void onPrepareClose () { }
        public void Close () {
            this.onPrepareClose ();
            this.mainWindowGameObject.SetActive (false);
        }
    }
}