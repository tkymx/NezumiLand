using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public class SelectMode : IGameMode {
        public void OnEnter () { 
            GameManager.Instance.GameUIManager.SelectModeUIPresenter.Show();
        }
        public void OnUpdate () { 
            if (GameManager.Instance.EventManager.IsToEvent) {
                GameManager.Instance.EventManager.GoToEvent();
            }
        }
        public void OnExit () { 
            GameManager.Instance.GameUIManager.SelectModeUIPresenter.Close();
        }
    }
}