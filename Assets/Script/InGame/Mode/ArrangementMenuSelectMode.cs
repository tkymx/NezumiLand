using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class ArrangementMenuSelectMode : IGameMode
    {
        private IArrangementTarget arrangementTarget;

        public ArrangementMenuSelectMode(IArrangementTarget arrangementTarget)
        {
            this.arrangementTarget = arrangementTarget;
        }

        public void OnEnter()
        {
            GameManager.Instance.GameUIManager.FieldActionUIPresenter.Show();
            GameManager.Instance.ArrangementManager.Select(arrangementTarget);
            GameManager.Instance.ArrangementPresenter.ReLoad();
        }
        public void OnUpdate()
        {
        }
        public void OnExit()
        {
            GameManager.Instance.GameUIManager.FieldActionUIPresenter.Close();
            GameManager.Instance.ArrangementManager.RemoveSelection();
            GameManager.Instance.ArrangementPresenter.ReLoad();
        }
    }
}
