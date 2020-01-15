using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public class ArrangementMenuSelectMode : IGameMode {
        private ArrangementMenuSelectModeContext context;

        public ArrangementMenuSelectMode (ArrangementMenuSelectModeContext context) {
            this.context = context;
        }

        public void OnEnter () {
            GameManager.Instance.ArrangementManager.Select (context.ArrangementTarget);
            GameManager.Instance.ArrangementPresenter.ReLoad ();
        }
        public void OnUpdate () { }
        public void OnExit () {
            GameManager.Instance.ArrangementManager.RemoveSelection ();
            GameManager.Instance.ArrangementPresenter.ReLoad ();
        }
    }
}