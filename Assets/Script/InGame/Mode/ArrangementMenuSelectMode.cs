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
            GameManager.Instance.GameUIManager.FieldActionUIPresenter.Show ();
            GameManager.Instance.ArrangementManager.Select (context.ArrangementTarget);
            GameManager.Instance.ArrangementPresenter.ReLoad ();
            GameManager.Instance.TimeManager.Pause ();
        }
        public void OnUpdate () { }
        public void OnExit () {
            GameManager.Instance.GameUIManager.FieldActionUIPresenter.Close ();
            GameManager.Instance.ArrangementManager.RemoveSelection ();
            GameManager.Instance.ArrangementPresenter.ReLoad ();
            GameManager.Instance.TimeManager.Play ();
        }
    }
}