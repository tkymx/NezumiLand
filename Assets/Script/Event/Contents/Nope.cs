using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL.EventContents {

    /// <summary>
    /// 何もしない
    /// お願いをクリアしたときの報酬付与などを想定
    /// </summary>
    public class Nope : EventContentsBase {
        public override EventContentsType EventContentsType => EventContentsType.Nope;

        public Nope(PlayerEventModel playerEventModel) : base(playerEventModel) {            
        }

        public override string ToString() {
            return this.EventContentsType.ToString();
        }
    }
}