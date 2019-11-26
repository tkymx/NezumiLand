using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace NL.EventContents {
    public class ForceConversation : IEventContents {
        private readonly PlayerEventModel playerEventModel = null;
        private readonly ConversationModel conversationModel = null;
        private Boolean isAlive = true;
        private IDisposable conversationDisposable = null;

        public ForceConversation(PlayerEventModel playerEventModel)
        {
            this.playerEventModel = playerEventModel;

            var conversationRepository = new ConversationRepository(ContextMap.DefaultMap);
            var conversationId = uint.Parse(playerEventModel.EventModel.EventContentsModel.Arg[0]);
            this.conversationModel = conversationRepository.Get(conversationId);

            this.isAlive = true;
            this.conversationDisposable = null;
        }

        public EventContentsType EventContentsType => EventContentsType.ForceConversation;

        public PlayerEventModel TargetPlayerEventModel => this.playerEventModel;

        public void OnEnter() {
            this.isAlive = true;
            this.conversationDisposable = ConversationStarter.StartConversation(this.conversationModel).Subscribe(_ => {
                this.isAlive = false;
            });
        }
        public void OnUpdate() {
        }
        public void OnExit() {
            this.conversationDisposable.Dispose();
        }
        public bool IsAvilve() {
            return isAlive;
        }

        public override string ToString() {
            return this.EventContentsType.ToString() + " " + conversationModel.Id.ToString();
        }
    }
}