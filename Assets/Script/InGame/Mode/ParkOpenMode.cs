using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace NL {
    public class ParkOpenMode : IGameMode {

        IDisposable disposable = null;

        public void OnEnter () { 
            GameManager.Instance.FieldRaycastManager.SetMaskMode(FieldRaycastManager.MaskMode.ParkOpen);

            this.disposable = GameManager.Instance.ParkOpenManager.OnCompleted
                .Subscribe(_ => {
                    GameManager.Instance.GameModeManager.EnqueueChangeMode(GameModeGenerator.GenerateSelectMode());
                });
        }
        public void OnUpdate () { 
        }
        public void OnExit () { 
            GameManager.Instance.FieldRaycastManager.SetMaskMode(FieldRaycastManager.MaskMode.All);

            if (this.disposable != null) {
                this.disposable.Dispose();
                this.disposable = null;
            }
        }
    }
}