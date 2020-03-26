using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace NL {
    public class SelectMode : IGameMode {

        private List<IDisposable> disposables = new List<IDisposable>();

        public void OnEnter () { 
            GameManager.Instance.GameUIManager.SelectModeUIPresenter.Show();

            // パークオープンの選択を実行
            this.disposables.Add(GameManager.Instance.GameUIManager.SelectModeUIPresenter.OnClickParkOpenSelectObservable.Subscribe(_ => {
                GameManager.Instance.ParkOpenGroupSelectManager.StartSelect();
            }));

            // 配置物がタップされたらメニューを表示
            this.disposables.Add(GameManager.Instance.ArrangementPresenter.OnTouchArrangement.Subscribe(arrangementTarget => {
                GameManager.Instance.GameModeManager.EnqueueChangeMode (GameModeGenerator.GenerateArrangementMenuSelectMode (arrangementTarget));
            }));
        }
        public void OnUpdate () { 
            if (GameManager.Instance.EventManager.IsToEvent) {
                GameManager.Instance.EventManager.GoToEvent();
            }
        }
        public void OnExit () { 
            GameManager.Instance.GameUIManager.SelectModeUIPresenter.Close();

            foreach (var disposable in this.disposables)
            {
                disposable.Dispose();
            }
        }
    }
}