using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace NL {
    public class ConversationMode : IGameMode {

        private IDisposable conversationDisposable = null;
        private ConversationModel conversationModel = null;

        public ConversationMode(ConversationModel conversationModel)
        {
            this.conversationDisposable = null;
            this.conversationModel = conversationModel;
        }

        public void OnEnter () { 
            GameManager.Instance.TimeManager.Pause ();
            this.conversationDisposable = ConversationStarter.StartConversation(this.conversationModel).Subscribe(_ => {
                GameManager.Instance.GameModeManager.Back();
            });
        }
        public void OnUpdate () {
        }
        public void OnExit () { 
            GameManager.Instance.TimeManager.Play ();
            if (this.conversationDisposable != null) {
                this.conversationDisposable.Dispose();
            } 
        }
    }
}
