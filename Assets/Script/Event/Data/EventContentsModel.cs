using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public enum EventContentsType {
        InValid,
        ForceConversation,
        TapCharacterConversation
    }

    public class EventContentsModel {
        public uint Id { get; private set; }
        public EventContentsType EventContentsType { get; private set; }
        public string[] Arg { get; private set; }

        public EventContentsModel (
            uint id,
            EventContentsType eventContentsType,
            string[] arg
        ) {
            this.Id = id;
            this.EventContentsType = eventContentsType;
            this.Arg = arg;
        }
    }
}