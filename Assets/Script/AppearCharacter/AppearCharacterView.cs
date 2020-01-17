using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public class AppearCharacterView : SelectBase {

        [SerializeField]
        SimpleAnimation simpleAnimation = null;


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

        public void SetPosition(Vector3 position) {
            this.transform.position = position;
        }

        public void SetRotation(Quaternion rotation) {
            this.transform.rotation = rotation;
        }

        public void ChangeAnimation(string tag) {
            simpleAnimation.CrossFade (tag, 0.5f);
        }
    }
}
