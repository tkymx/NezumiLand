using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public class AppearCharacterIndicatorView : MonoBehaviour {

        [SerializeField]
        private GameObject conversationNotifier = null;

        [SerializeField]
        private GameObject unlockOnegai = null;

        [SerializeField]
        private GameObject clearOnegai = null;

        public void Initialize ()
        {
            conversationNotifier.SetActive(false);
            unlockOnegai.SetActive(false);
            clearOnegai.SetActive(false);
        }

        public void SetConversationNotifierEnabled(bool isEnabled) {
            this.conversationNotifier.SetActive(isEnabled);
        }

        public void DisableOnegaiIndicator() {
            this.unlockOnegai.SetActive(false);
            this.clearOnegai.SetActive(false);
        }

        public void EnableOnegaiIndicator(OnegaiState onegaiState) {
            DisableOnegaiIndicator();
            if (onegaiState == OnegaiState.UnLock || onegaiState == OnegaiState.Lock) {
                this.unlockOnegai.SetActive(true);
            }
            else if (onegaiState == OnegaiState.Clear) {
                this.clearOnegai.SetActive(true);
            }
        }
    }
}
