using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public class AppearCharacterView : MonoBehaviour {
        private TypeObservable<int> onSelect = null;
        public TypeObservable<int> OnSelect => onSelect;

        private void Awake () {
            this.onSelect = new TypeObservable<int> ();
        }
    }
}
