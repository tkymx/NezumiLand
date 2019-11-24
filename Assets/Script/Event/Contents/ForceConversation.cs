using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL.EventContents {
    public class ForceConversation : IEventContents {
        private uint conversationId;
        public ForceConversation(EventContentsModel eventContentsModel)
        {
            conversationId = uint.Parse(eventContentsModel.Arg[0]);
        }

        public EventContentsType EventContentsType => EventContentsType.ForceConversation;

        public void OnEnter() {

        }
        public void OnUpdate() {

        }
        public void OnExit() {

        }
        public bool IsAvilve() {
            return true;
        }
    }
}