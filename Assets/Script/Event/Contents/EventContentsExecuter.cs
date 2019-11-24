using System.Collections;
using System.Collections.Generic;
using NL.EventContents;
using UnityEngine;
using System.Linq;

namespace NL {
    public class EventContentsExecuter {
        private readonly IPlayerEventRepository playerEventRepository = null;
        private IEventContents currentEventContents = null;
        public IEventContents CurrentEventContents => currentEventContents;
        public EventContentsExecuter(IPlayerEventRepository playerEventRepository) {
            this.currentEventContents = new Invalid();
            this.playerEventRepository = playerEventRepository;
        }
        /// <summary>
        /// コンテンツが実行中かどうか？
        /// </summary>
        public bool HasContents => currentEventContents.EventContentsType != EventContentsType.InValid;
        public void UpdateByFrame() {
            Debug.Assert(currentEventContents != null, "currentEventContentsがありません");
            this.currentEventContents.OnUpdate();
            if (!this.currentEventContents.IsAvilve()) {
                PlayNext();
            }
        }
        /// <summary>
        /// 次の状態に進む
        /// </summary>
        public void PlayNext() {
            var clearEvent = this.playerEventRepository.GetClearEvent()
                .Where(eventModel => {
                    if (this.currentEventContents.TargetPlayerEventModel == null) {
                        return true;
                    }
                    return eventModel.Id != this.currentEventContents.TargetPlayerEventModel.Id;
                });
            if (!clearEvent.Any()) {
                PlayContents(new Invalid());
                return;
            }
            PlayContents(EventContentsGenerator.Generate(clearEvent.First()));
        }
        public void PlayContents(IEventContents eventContentes) {
            Debug.Assert(this.currentEventContents != null, "currentEventContentsがありません");
            this.currentEventContents.OnExit();  
            // 終わった段階で終了状態にする            
            if (this.currentEventContents.TargetPlayerEventModel != null) {
                this.currentEventContents.TargetPlayerEventModel.ToDone();
                playerEventRepository.Store(this.currentEventContents.TargetPlayerEventModel);
            }
            this.currentEventContents = eventContentes;
            eventContentes.OnEnter();
        }
    }
}
