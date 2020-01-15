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
            this.conversationDisposable = ConversationStarter.StartConversation(this.conversationModel).Subscribe(_ => {
                GameManager.Instance.GameModeManager.Back();
            });
        }
        public void OnUpdate () {
        }
        public void OnExit () { 
            if (this.conversationDisposable != null) {
                this.conversationDisposable.Dispose();
            } 
        }
    }
}
