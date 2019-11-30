using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace NL.EventContents {
    public abstract class EventContentsBase : IEventContents {
        private readonly PlayerEventModel playerEventModel = null;

        public EventContentsBase(PlayerEventModel playerEventModel)
        {
            this.playerEventModel = playerEventModel;
        }

        public abstract EventContentsType EventContentsType { get; }
        public PlayerEventModel TargetPlayerEventModel => this.playerEventModel;
        
        public virtual void OnEnter() {            
        }
        public virtual void OnUpdate() {            
        }
        public virtual void OnExit() {
        }
        public virtual bool IsAlive(){
            return false;
        }
    }
}