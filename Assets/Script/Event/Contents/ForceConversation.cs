using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace NL.EventContents {
    public class ForceConversation : EventContentsBase {
        private readonly ConversationModel conversationModel = null;
        private Boolean isAlive = true;
        private IDisposable conversationDisposable = null;

        public ForceConversation(PlayerEventModel playerEventModel) : base(playerEventModel)
        {
            var conversationRepository = new ConversationRepository(ContextMap.DefaultMap);
            var conversationId = uint.Parse(playerEventModel.EventModel.EventContentsModel.Arg[0]);
            this.conversationModel = conversationRepository.Get(conversationId);

            this.isAlive = true;
            this.conversationDisposable = null;
        }

        public override EventContentsType EventContentsType => EventContentsType.ForceConversation;

        public override void OnEnter() {
            this.isAlive = true;
            this.conversationDisposable = ConversationStarter.StartConversation(this.conversationModel).Subscribe(_ => {
                this.isAlive = false;
            });
        }
        public override void OnUpdate() {
        }
        public override void OnExit() {
            this.conversationDisposable.Dispose();
        }
        public override bool IsAlive() {
            return isAlive;
        }

        public override string ToString() {
            return this.EventContentsType.ToString() + " " + conversationModel.Id.ToString();
        }
    }
}