using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace NL {

    /// <summary>
    /// イベントモード
    /// - イベント実行時のpre,post処理などを担当
    /// </summary>
    public class MousePurchaseMode : IGameMode {

        public void OnEnter () {
            GameManager.Instance.GameUIManager.MousePurchasePresenter.Show();
        }
        public void OnUpdate () {
            // 表示していなかったら戻す
            if (!GameManager.Instance.GameUIManager.MousePurchasePresenter.IsShow ()) {
                GameManager.Instance.GameModeManager.Back();
            }            
        }
        public void OnExit () {
            GameManager.Instance.GameUIManager.MousePurchasePresenter.Close();
        }
    }
}