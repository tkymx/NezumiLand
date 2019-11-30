using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    /// <summary>
    /// イベントの管理
    /// - なにか動作が起こるたびにイベントの判定を行う
    /// - コンテンツの実行を行う
    /// </summary>
    public class EventManager {        
        private EventConditionDetecter eventConditionDetecter = null;
        private EventContentsExecuter eventContentsExecuter = null;

        public IEventContents CurrentEventContents => eventContentsExecuter.CurrentEventContents;
        public bool IsToEvent { get; private set; } 

        public EventManager(IPlayerEventRepository playerEventRepository) {
            this.eventConditionDetecter = new EventConditionDetecter(playerEventRepository);
            this.eventContentsExecuter = new EventContentsExecuter(playerEventRepository);
            this.IsToEvent = false;
        }
        
        public void UpdateByFrame() {
            this.eventContentsExecuter.UpdateByFrame();
        }

        public bool IsEventContentsPlaying() {
            return this.eventContentsExecuter.HasPlayingContents;
        }

        public void PlayEventContents() {
            this.eventContentsExecuter.PlayNext();
        }

        public void GoToEvent() {            
            GameManager.Instance.GameModeManager.EnqueueChangeModeWithHistory(GameModeGenerator.GenerateEventMode());    
            this.IsToEvent = false;
        }

        public void PushEventParameter(IEventCondition eventCondtion) {
            this.eventConditionDetecter.Detect(eventCondtion);
            this.ChangeEventModeIfNessesary();
        }

        private void ChangeEventModeIfNessesary () {
            if (GameManager.Instance.GameModeManager.IsEventMode) {
                return;
            }
            if (this.eventContentsExecuter.HasPlayingContents) {       
                return;
            }
            if (!this.eventContentsExecuter.HasPlayableEvent()) {
                return;
            }
            this.IsToEvent = true;
        }
    }
}