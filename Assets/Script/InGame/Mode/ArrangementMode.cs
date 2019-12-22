using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NL {
    public class ArrangementMode : IGameMode {
        private Camera mainCamera;
        private ArrangementModeContext context;

        public ArrangementMode (Camera mainCamera, ArrangementModeContext context) {
            this.mainCamera = mainCamera;
            this.context = context;
        }

        public void OnEnter () {
            GameManager.Instance.GameUIManager.FieldActionUIPresenter.Show ();
            GameManager.Instance.ArrangementManager.ArrangementAnnotater.RemoveAllAnnotation ();
            GameManager.Instance.MonoSelectManager.SelectMonoInfo (context.TargetMonoInfo);
            GameManager.Instance.TimeManager.Pause ();
        }

        public void OnUpdate () {
            OrderMouseIfSelect();
        }

        private void OrderMouseIfSelect () {
            if (!GameManager.Instance.ArrangementManager.ArrangementAnnotater.IsSelectByFrame) {
                return;
            }

            var currentTarget = GameManager.Instance.ArrangementManager.ArrangementAnnotater.GetCurrentTarget ();
            if (!GameManager.Instance.ArrangementManager.IsSetArrangement (currentTarget)) {
                return;
            }

            var makingMono = GameManager.Instance.MonoSelectManager.SelectedMonoInfo;
            var makingArrangementResourceAmount = makingMono.ArrangementResourceAmount;
            if (!ArrangementResourceHelper.IsConsume (makingArrangementResourceAmount).IsConsume) {
                GameManager.Instance.EffectManager.PlayError ("素材が足りません。", currentTarget.CenterPosition);
                return;
            }

            ReserveArrangementService.Execute(currentTarget, makingMono);
        }

        public void OnExit () {
            GameManager.Instance.GameUIManager.FieldActionUIPresenter.Close ();
            GameManager.Instance.ArrangementManager.ArrangementAnnotater.RemoveAllAnnotation ();
            GameManager.Instance.MonoSelectManager.RemoveSelect ();
            GameManager.Instance.TimeManager.Play ();
        }
    }
}