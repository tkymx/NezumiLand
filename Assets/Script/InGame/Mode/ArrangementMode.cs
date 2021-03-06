using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

namespace NL {
    public class ArrangementMode : IGameMode {
        private Camera mainCamera;
        private ArrangementModeContext context;
        private ArrangementTargetCreateService arrangementTargetCreateService = null;
        private ReserveArrangementService reserveArrangementService = null;
        private List<IDisposable> disposables;

        public ArrangementMode (Camera mainCamera, ArrangementModeContext context) {
            this.mainCamera = mainCamera;
            this.context = context;
            this.disposables = new List<IDisposable>();

            this.arrangementTargetCreateService = new ArrangementTargetCreateService(context.PlayerArrangementTargetRepository);
            this.reserveArrangementService = new ReserveArrangementService(context.PlayerArrangementTargetRepository);
        }

        public void OnEnter () {
            GameManager.Instance.GameUIManager.FieldActionUIPresenter.Show ();
            GameManager.Instance.GameUIManager.ArrangementModeUIPresenter.Show ();
            GameManager.Instance.FieldRaycastManager.SetMaskMode(FieldRaycastManager.MaskMode.Field);
            GameManager.Instance.ArrangementManager.ArrangementAnnotater.RemoveAllAnnotation ();
            GameManager.Instance.MonoSelectManager.SelectMonoInfo (context.TargetMonoInfo);
            GameManager.Instance.TimeManager.Pause ();
            GameManager.Instance.CameraMoveManager.ChangeMode(CameraMoveManager.CameraMode.Arrangement);

            this.disposables.Add(GameManager.Instance.ArrangementManager.ArrangementAnnotater.OnSelect.Subscribe(_ => {
                OrderMouseIfSelect();
            }));

            GameManager.Instance.EventManager.PushEventParameter(new NL.EventCondition.InArrangementMode());
        }

        public void OnUpdate () {
        }

        private void OrderMouseIfSelect () {
            var currentTarget = GameManager.Instance.ArrangementManager.ArrangementAnnotater.GetCurrentTarget ();
            if (!GameManager.Instance.ArrangementManager.IsSetArrangement (currentTarget)) {

                return;
            }

            var makingMono = GameManager.Instance.MonoSelectManager.SelectedMonoInfo;
            var makingArrangementResourceAmount = makingMono.ArrangementResourceAmount;
            var result = ArrangementResourceHelper.IsConsume (makingArrangementResourceAmount);
            if (!result.IsConsume) {
                GameManager.Instance.EffectManager.PlayError (result.GetErrorMessage(), currentTarget.CenterPosition);
                return;
            }

            // 追加して予約する
            var playerArrangementTarget = this.arrangementTargetCreateService.Execute(currentTarget);
            this.reserveArrangementService.Execute(playerArrangementTarget, makingMono);
        }

        public void OnExit () {
            GameManager.Instance.GameUIManager.FieldActionUIPresenter.Close ();
            GameManager.Instance.GameUIManager.ArrangementModeUIPresenter.Close ();
            GameManager.Instance.FieldRaycastManager.SetMaskMode(FieldRaycastManager.MaskMode.All);
            GameManager.Instance.ArrangementManager.ArrangementAnnotater.RemoveAllAnnotation ();
            GameManager.Instance.MonoSelectManager.RemoveSelect ();
            GameManager.Instance.TimeManager.Play ();
            GameManager.Instance.CameraMoveManager.ChangeMode(CameraMoveManager.CameraMode.Normal);

            foreach (var disposable in this.disposables)
            {
                disposable.Dispose();
            }
            this.disposables.Clear();
        }
    }
}