using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public class OnegaiSelectMode : IGameMode {

        public OnegaiSelectMode () {
        }

        public void OnEnter () {
            GameManager.Instance.GameUIManager.FieldActionUIPresenter.Show ();
            GameManager.Instance.GameUIManager.OnegaiPresenter.Show ();
            GameManager.Instance.TimeManager.Pause ();
            GameManager.Instance.GameUIManager.OnegaiPresenter.OnClose
                .Subscribe(_ => {
                    GameManager.Instance.GameModeManager.EnqueueChangeMode(GameModeGenerator.GenerateSelectMode());
                });
        }
        public void OnUpdate () {
        }
        public void OnExit () {
            GameManager.Instance.GameUIManager.FieldActionUIPresenter.Close ();
            GameManager.Instance.GameUIManager.OnegaiPresenter.Close ();
            GameManager.Instance.TimeManager.Play ();
        }
    }
}