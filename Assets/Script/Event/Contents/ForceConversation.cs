using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace NL.EventContents {
    public class ForceConversation : EventContentsBase {
        private readonly ConversationModel conversationModel = null;
        private Boolean isAlive = true;

        public ForceConversation(PlayerEventModel playerEventModel) : base(playerEventModel)
        {
            var conversationRepository = new ConversationRepository(ContextMap.DefaultMap);
            var conversationId = uint.Parse(playerEventModel.EventModel.EventContentsModel.Arg[0]);
            this.conversationModel = conversationRepository.Get(conversationId);

            this.isAlive = true;
        }

        public override EventContentsType EventContentsType => EventContentsType.ForceConversation;

        public override void OnEnter() {
            this.isAlive = true;
            var conversationMode = GameModeGenerator.GenerateConversationMode(this.conversationModel);
            GameManager.Instance.GameModeManager.EnqueueChangeModeWithHistory(conversationMode);
            GameManager.Instance.GameModeManager.GetModeEndObservable(conversationMode).Subscribe(_ => {
                this.isAlive = false;
            });
        }
        public override void OnUpdate() {
        }
        public override void OnExit() {
        }
        public override bool IsAlive() {
            return isAlive;
        }

        public override string ToString() {
            return this.EventContentsType.ToString() + " " + conversationModel.Id.ToString();
        }
    }
}