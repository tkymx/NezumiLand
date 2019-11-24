using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public class MouseSelect : SelectBase {
        private TypeObservable<int> onMouseSelet;
        public TypeObservable<int> OnMouseSelect => onMouseSelet;

        private void Awake () {
            this.onMouseSelet = new TypeObservable<int> ();
        }

        public override void OnOver (RaycastHit hit) {

        }
        public override void OnSelect (RaycastHit hit) {
            this.onMouseSelet.Execute (0);
        }
    }
}