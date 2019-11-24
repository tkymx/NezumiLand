using System.Collections;
using System.Collections.Generic;
using NL.EventContents;
using UnityEngine;
using System.Linq;

namespace NL {
    public class EventContentsExecuter {
        private readonly IPlayerEventRepository playerEventRepository = null;
        private IEventContents currentEventContents = null;
        public EventContentsType CurrentEventContentsType => currentEventContents.EventContentsType;
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
            var clearEvent = this.playerEventRepository.GetClearEvent();            
            if (!clearEvent.Any()) {
                PlayContents(new Invalid());
                return;
            }
            PlayContents(EventContentsGenerator.Generate(clearEvent.First().EventModel.EventContentsModel));
        }
        public void PlayContents(IEventContents eventContentes) {
            Debug.Assert(this.currentEventContents != null, "currentEventContentsがありません");
            this.currentEventContents.OnExit();
            this.currentEventContents = eventContentes;
            eventContentes.OnEnter();
        }
    }
}
