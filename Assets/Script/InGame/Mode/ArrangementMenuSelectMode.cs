using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace NL {
    public class ArrangementMenuSelectMode : IGameMode {
        private ArrangementMenuSelectModeContext context;
        private List<IDisposable> disposables;

        public ArrangementMenuSelectMode (ArrangementMenuSelectModeContext context) {
            this.context = context;
            this.disposables = new List<IDisposable>();
        }

        public void OnEnter () {
            GameManager.Instance.ArrangementManager.SelectOnly (context.ArrangementTarget);
            GameManager.Instance.ArrangementPresenter.ReLoad ();

            // 配置物がタップされたらメニューを表示
            this.disposables.Add(GameManager.Instance.ArrangementPresenter.OnTouchArrangement.Subscribe(arrangementTarget => {
                GameManager.Instance.GameModeManager.EnqueueChangeMode (GameModeGenerator.GenerateArrangementMenuSelectMode (arrangementTarget));
            }));
        }
        public void OnUpdate () { }
        public void OnExit () {
            GameManager.Instance.ArrangementManager.RemoveSelection ();
            GameManager.Instance.ArrangementPresenter.ReLoad ();

            foreach (var disposable in this.disposables)
            {
                disposable.Dispose();
            }            
        }
    }
}