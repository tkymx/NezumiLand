using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    /// <summary>
    /// イベントの管理
    /// - なにか動作が起こるたびにイベントの判定を行う
    /// </summary>
    public class EventManager {        
        private EventConditionDetecter eventConditionDetecter = null;
        private EventContentsExecuter eventContentsExecuter = null;
        public IEventContents CurrentEventContents => eventContentsExecuter.CurrentEventContents;
        public EventManager(IPlayerEventRepository playerEventRepository) {
            this.eventConditionDetecter = new EventConditionDetecter(playerEventRepository);
            this.eventContentsExecuter = new EventContentsExecuter(playerEventRepository);
        }
        public void UpdateByFrame() {
            this.eventContentsExecuter.UpdateByFrame();
        }

        public void PushEventParameter(IEventCondtion eventCondtion) {
            this.eventConditionDetecter.Detect(eventCondtion);

            // 実行中のイベントが無ければ実行する
            if (!this.eventContentsExecuter.HasContents) {
                this.eventContentsExecuter.PlayNext();
            }
        }
    }
}