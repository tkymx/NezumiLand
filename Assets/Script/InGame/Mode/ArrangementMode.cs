using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NL
{
    public class ArrangementMode : IGameMode
    {
        private Camera mainCamera;
        private ArrangementModeContext context;

        public ArrangementMode(Camera mainCamera, ArrangementModeContext context)
        {
            this.mainCamera = mainCamera;
            this.context = context;
        }

        public void OnEnter()
        {
            GameManager.Instance.GameUIManager.FieldActionUIPresenter.Show();
            GameManager.Instance.ArrangementManager.ArrangementAnnotater.RemoveAllAnnotation();
            GameManager.Instance.ArrangementManager.Enable(context.TargetMonoInfo);
        }

        public void OnUpdate()
        {
        }

        public void OnExit()
        {
            GameManager.Instance.GameUIManager.FieldActionUIPresenter.Close();
            GameManager.Instance.ArrangementManager.ArrangementAnnotater.RemoveAllAnnotation();
            GameManager.Instance.ArrangementManager.Disable();
        }


    }
}