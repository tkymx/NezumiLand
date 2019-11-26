using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace NL {
    public class ConversationPresenter : UiWindowPresenterBase
    {
        [SerializeField]
        private ConversationView conversationView = null;

        [SerializeField]
        private ConversationCharacterView conversationCharacterView = null;

        private TypeObservable<int> onEndConversation = null;
        public TypeObservable<int> OnEndConversation => onEndConversation;

        private ConversationModel currentConversationModel = null;
        private int currentConversationIndex = 0;

        private IDisposable onEndConversationDisposable = null;

        public void Initialize()
        {
            // init
            this.conversationView.Initialize();
            this.onEndConversation = new TypeObservable<int>();

            // end conversation event
            this.onEndConversationDisposable = this.conversationView.OnEndConversation.Subscribe(_=>{
                if (currentConversationModel.IsLastIndex(this.currentConversationIndex)) {
                    this.Close();
                    this.onEndConversation.Execute(0);
                    return;
                }
                this.currentConversationIndex++;
                this.SetConversationIndex(this.currentConversationIndex);
            });

            // initialize close
            this.Close();
        }

        public void StartConversation(ConversationModel conversationModel) {
            this.currentConversationModel = conversationModel;
            this.currentConversationIndex = 0;
            this.SetConversationIndex(currentConversationIndex);
            this.Show();
        }

        private void SetConversationIndex(int index) {
            Debug.Assert(currentConversationModel!=null,"currentConversationModelがnullです。");
            this.conversationCharacterView.UpdateCharacterImage(this.currentConversationModel.ConversationCharacterNames[index]);
            this.conversationView.StartSpeak("サンプル", this.currentConversationModel.ConversationTexts[index]);
        }

        private void OnDestroy() {
            if (this.onEndConversationDisposable != null) {
                this.onEndConversationDisposable.Dispose();
                this.onEndConversationDisposable = null;
            }
        }
    }
}

