using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace NL {
    public class OnegaiSelectMode : IGameMode {

        private IDisposable disposable = null;

        public OnegaiSelectMode () {
        }

        public void OnEnter () {
            GameManager.Instance.GameUIManager.OnegaiPresenter.Show ();
            this.disposable = GameManager.Instance.GameUIManager.OnegaiPresenter.OnClose
                .Subscribe(_ => {
                    GameManager.Instance.GameModeManager.EnqueueChangeMode(GameModeGenerator.GenerateSelectMode());
                });
        }
        public void OnUpdate () {
        }
        public void OnExit () {
            if (this.disposable != null) {
                this.disposable.Dispose();
            }
            GameManager.Instance.GameUIManager.OnegaiPresenter.Close ();
        }
    }
}