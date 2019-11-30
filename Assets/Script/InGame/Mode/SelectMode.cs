using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public class SelectMode : IGameMode {
        public void OnEnter () { }
        public void OnUpdate () { 
            if (GameManager.Instance.EventManager.IsToEvent) {
                GameManager.Instance.EventManager.GoToEvent();
            }
        }
        public void OnExit () { }
    }
}