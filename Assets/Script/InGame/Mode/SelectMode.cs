using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public class SelectMode : IGameMode {

        public string UniqueKey() {
            return "SelectMode";
        }
        public void OnEnter () { }
        public void OnUpdate () { }
        public void OnExit () { }
    }
}