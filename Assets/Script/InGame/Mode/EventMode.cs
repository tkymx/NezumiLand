using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {

    /// <summary>
    /// イベントモード
    /// - イベント実行時のpre,post処理などを担当
    /// </summary>
    public class EventMode : IGameMode {

        public EventMode () {
        }
        
        public void OnEnter () {
            GameManager.Instance.TimeManager.Pause ();
            GameManager.Instance.EventManager.PlayEventContents();
        }
        public void OnUpdate () {     
            if (!GameManager.Instance.EventManager.IsEventContentsPlaying()) {
                GameManager.Instance.GameModeManager.Back();
            }     
        }
        public void OnExit () {
            GameManager.Instance.TimeManager.Play ();
        }
    }
}