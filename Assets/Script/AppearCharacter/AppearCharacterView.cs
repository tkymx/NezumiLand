using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public class AppearCharacterView : SelectBase {

        [SerializeField]
        private SimpleAnimation simpleAnimation = null;

        [SerializeField]
        private AppearCharacterIndicatorView appearCharacterIndicatorView = null;

        private TypeObservable<int> onSelectObservable = null;
        public TypeObservable<int> OnSelectObservable => onSelectObservable;

        private void Awake () {
            this.onSelectObservable = new TypeObservable<int> ();
            this.appearCharacterIndicatorView.Initialize();
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

        public void SetConversationNotifierEnabled(bool isEnabled) {
            this.appearCharacterIndicatorView.SetConversationNotifierEnabled(isEnabled);
        }

        public void DisableOnegaiIndicator() {
            this.appearCharacterIndicatorView.DisableOnegaiIndicator();
        }

        public void EnableOnegaiIndicator(OnegaiState onegaiState) {
            this.appearCharacterIndicatorView.EnableOnegaiIndicator(onegaiState);
        }
    }
}
