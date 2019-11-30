using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL.EventContents {
    public class Invalid : IEventContents {
        public EventContentsType EventContentsType => EventContentsType.InValid;

        public PlayerEventModel TargetPlayerEventModel => null;

        public void OnEnter() {

        }
        public void OnUpdate() {

        }
        public void OnExit() {

        }
        public bool IsAvilve() {
            return false;
        }

        public override string ToString() {
            return this.EventContentsType.ToString();
        }
    }
}