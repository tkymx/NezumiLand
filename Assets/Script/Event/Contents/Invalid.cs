using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL.EventContents {
    public class Invalid : EventContentsBase {
        
        public override EventContentsType EventContentsType => EventContentsType.InValid;

        public Invalid() : base(null) {            
        }

        public override string ToString() {
            return this.EventContentsType.ToString();
        }
    }
}