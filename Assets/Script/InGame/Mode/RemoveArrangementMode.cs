using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

namespace NL {

    /// <summary>
    /// 配置を削除するモード
    /// </summary>
    public class RemoveArrangementMode : IGameMode {
        private List<IDisposable> disposables;

        public RemoveArrangementMode () {
            this.disposables = new List<IDisposable>();
        }

        public void OnEnter () {
            GameManager.Instance.GameUIManager.RemoveArrangementModeUIPresenter.Show ();
            GameManager.Instance.FieldRaycastManager.SetMaskMode(FieldRaycastManager.MaskMode.RemoveArrangement);
            GameManager.Instance.TimeManager.Pause ();
            GameManager.Instance.CameraMoveManager.ChangeMode(CameraMoveManager.CameraMode.Arrangement);

            // 消去がされたら消去する
            this.disposables.Add(GameManager.Instance.GameUIManager.RemoveArrangementModeUIPresenter.OnRemoveObservable.Subscribe(_ => 
            {
                foreach (var selectedArrangementTarget in GameManager.Instance.ArrangementManager.SelectedArrangementTargets)
                {
                    Debug.Assert(GameManager.Instance.Wallet.IsConsume(selectedArrangementTarget.MonoInfo.RemoveFee), "消費できません");
                    
                    // 資源の増減
                    GameManager.Instance.Wallet.Consume (selectedArrangementTarget.MonoInfo.RemoveFee);
                    GameManager.Instance.ArrangementItemStore.Push (selectedArrangementTarget.MonoInfo.ArrangementItemAmount);
                    // 増減の表示
                    GameManager.Instance.EffectManager.PlayConsumeEffect (selectedArrangementTarget.MonoInfo.RemoveFee, selectedArrangementTarget.CenterPosition);
                    GameManager.Instance.EffectManager.PlayEarnItemEffect (selectedArrangementTarget.MonoInfo.ArrangementItemAmount, selectedArrangementTarget.CenterPosition + new Vector3(0,2.0f,0));
                }
                GameManager.Instance.ArrangementManager.RemoveSelectArrangement ();
            }));

            // 配置物がタップされたら選択
            this.disposables.Add(GameManager.Instance.ArrangementPresenter.OnTouchArrangement.Subscribe(arrangementTarget => {
                if (GameManager.Instance.ArrangementManager.CheckIsSelect(arrangementTarget)) {
                    GameManager.Instance.ArrangementManager.RemoveSelection(arrangementTarget);
                } else {
                    GameManager.Instance.ArrangementManager.Select(arrangementTarget);
                }
            }));

            // 終了時はセバックする
            this.disposables.Add(GameManager.Instance.GameUIManager.RemoveArrangementModeUIPresenter.OnClose.Subscribe(_ => {
                GameManager.Instance.GameModeManager.EnqueueChangeMode (GameModeGenerator.GenerateMenuSelectMode ());
            }));

        }

        public void OnUpdate () {
        }

        public void OnExit () {
            GameManager.Instance.GameUIManager.RemoveArrangementModeUIPresenter.Close ();
            GameManager.Instance.FieldRaycastManager.SetMaskMode(FieldRaycastManager.MaskMode.All);
            GameManager.Instance.TimeManager.Play ();
            GameManager.Instance.ArrangementManager.RemoveSelection();
            GameManager.Instance.CameraMoveManager.ChangeMode(CameraMoveManager.CameraMode.Normal);

            foreach (var disposable in this.disposables)
            {
                disposable.Dispose();
            }
        }
    }
}