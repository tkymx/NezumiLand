using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL.EventContents {
    public class ForceConversation : IEventContents {
        private PlayerEventModel playerEventModel;
        private uint conversationId;
        public ForceConversation(PlayerEventModel playerEventModel)
        {
            this.playerEventModel = playerEventModel;
            this.conversationId = uint.Parse(playerEventModel.EventModel.EventContentsModel.Arg[0]);
        }

        public EventContentsType EventContentsType => EventContentsType.ForceConversation;

        public PlayerEventModel TargetPlayerEventModel => this.playerEventModel;

        private float elapsedTime = 0;

        public void OnEnter() {
            this.elapsedTime = 0;
        }
        public void OnUpdate() {
            this.elapsedTime += GameManager.Instance.TimeManager.DeltaTime();
        }
        public void OnExit() {

        }
        public bool IsAvilve() {
            return this.elapsedTime < 10;
        }

        public override string ToString() {
            return this.EventContentsType.ToString() + " " + elapsedTime.ToString();
        }
    }
}