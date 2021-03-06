﻿using System.Collections;
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

        public void Initialize()
        {
            // init
            this.conversationView.Initialize();
            this.onEndConversation = new TypeObservable<int>();

            // end conversation event
            this.disposables.Add(this.conversationView.OnEndConversation.Subscribe(_=>{
                if (this.IsEndConversation()) {
                    this.Close();
                    this.onEndConversation.Execute(0);
                    return;
                }
                this.currentConversationIndex++;
                this.SetConversationIndex(this.currentConversationIndex);
            }));

            // initialize close
            this.Close();
        }

        public void StartConversation(ConversationModel conversationModel) {
            Debug.Assert(IsEndConversation(), "まだ会話が終了していません");

            this.currentConversationModel = conversationModel;
            this.currentConversationIndex = 0;
            this.SetConversationIndex(currentConversationIndex);
            this.Show();

            // 前の会話に反応する可能性があるので初期化
            this.onEndConversation = new TypeObservable<int>();
        }

        private bool IsEndConversation()
        {
            if (this.currentConversationModel == null) {
                return true;
            }

            return currentConversationModel.IsLastIndex(this.currentConversationIndex);
        }

        private void SetConversationIndex(int index) {
            Debug.Assert(currentConversationModel!=null,"currentConversationModelがnullです。");
            this.conversationCharacterView.UpdateCharacterImage(this.currentConversationModel.ConversationCharacterNames[index]);
            this.conversationView.StartSpeak(this.currentConversationModel.ConversationCharacterDisplayNames[index], this.currentConversationModel.ConversationTexts[index]);
        }
    }
}

