using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public class AppearCharacterView : SelectBase {
        private TypeObservable<int> onSelectObservable = null;
        public TypeObservable<int> OnSelectObservable => onSelectObservable;

        private void Awake () {
            this.onSelectObservable = new TypeObservable<int> ();
        }

        public override void OnOver (RaycastHit hit) {
        }

        public override void OnSelect (RaycastHit hit) {
            this.onSelectObservable.Execute (0);
        }
    }
}
