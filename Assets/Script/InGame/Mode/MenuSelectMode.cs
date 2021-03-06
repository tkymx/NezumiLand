using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace NL {
    public class MenuSelectMode : IGameMode {
        private MenuSelectModeContext context;
        private List<IDisposable> disposables;

        public MenuSelectMode (MenuSelectModeContext context) {
            this.context = context;
            this.disposables = new List<IDisposable>();
        }

        public void OnEnter () {
            GameManager.Instance.GameUIManager.MonoTabPresenter.Show ();

            GameManager.Instance.EventManager.PushEventParameter(new NL.EventCondition.InMonoSelectMode());
        }
        public void OnUpdate () {
            if (!GameManager.Instance.MonoSelectManager.HasSelectedMonoInfo) {
                return;
            }

            Debug.Assert (GameManager.Instance.MouseStockManager.IsConsume (), "オーダーできないのにモノが選択されました。");

            // なんかここで入れてあげるのは違う気もする
            this.context.SelectMonoInfo (GameManager.Instance.MonoSelectManager.SelectedMonoInfo);

            GameManager.Instance.GameModeManager.EnqueueChangeMode (GameModeGenerator.GenerateArrangementMode ());
        }
        public void OnExit () {
            GameManager.Instance.GameUIManager.MonoTabPresenter.Close ();
        }
    }
}