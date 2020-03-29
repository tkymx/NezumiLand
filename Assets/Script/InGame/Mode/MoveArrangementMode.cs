using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

namespace NL {

    /// <summary>
    /// 配置を削除するモード
    /// </summary>
    public class MoveArrangementMode : IGameMode {
        private List<IDisposable> disposables = null;

        public MoveArrangementMode () {
            this.disposables = new List<IDisposable>();
        }

        public void OnEnter () {
            GameManager.Instance.GameUIManager.MoveArrangementModeUIPresenter.Show ();
            GameManager.Instance.FieldRaycastManager.SetMaskMode(FieldRaycastManager.MaskMode.MoveArrangement);
            GameManager.Instance.TimeManager.Pause ();
            GameManager.Instance.CameraMoveManager.ChangeMode(CameraMoveManager.CameraMode.Arrangement);
            GameManager.Instance.ArrangementPresenter.ChangeToMoveArrangementDeployMode();

            // 配置物がタップされたら
            this.disposables.Add(GameManager.Instance.ArrangementPresenter.OnTouchArrangement.Subscribe(arrangementTarget => {
                GameManager.Instance.ArrangementMoveIndicatorManager.Start(arrangementTarget);
            }));

            // 終了時はセバックする
            this.disposables.Add(GameManager.Instance.GameUIManager.MoveArrangementModeUIPresenter.OnClose.Subscribe(_ => {
                GameManager.Instance.ArrangementMoveIndicatorManager.End();
                GameManager.Instance.GameModeManager.EnqueueChangeMode (GameModeGenerator.GenerateMenuSelectMode ());
            }));

            // フィールドタップ時
            this.disposables.Add(GameManager.Instance.ArrangementManager.ArrangementAnnotater.OnSelect.Subscribe(_ => {
                // 移動用の ArrangmentTargetを作成する
                var currentTarget = GameManager.Instance.ArrangementManager.ArrangementAnnotater.GetCurrentTarget ();
                if (!GameManager.Instance.ArrangementManager.IsSetArrangement (currentTarget)) {
                    return;
                }
                // 移動する
                GameManager.Instance.ArrangementMoveIndicatorManager.MoveIndicatorPosition(currentTarget.ArrangementPositions);
                GameManager.Instance.ArrangementPresenter.ReLoad();
            }));
        }

        public void OnUpdate () {
        }

        public void OnExit () {
            GameManager.Instance.GameUIManager.MoveArrangementModeUIPresenter.Close ();
            GameManager.Instance.FieldRaycastManager.SetMaskMode(FieldRaycastManager.MaskMode.All);
            GameManager.Instance.TimeManager.Play ();
            GameManager.Instance.ArrangementManager.RemoveSelection();
            GameManager.Instance.CameraMoveManager.ChangeMode(CameraMoveManager.CameraMode.Normal);
            GameManager.Instance.ArrangementPresenter.ChangeToArrangementDeployMode();

            foreach (var disposable in this.disposables)
            {
                disposable.Dispose();
            }
        }
    }
}