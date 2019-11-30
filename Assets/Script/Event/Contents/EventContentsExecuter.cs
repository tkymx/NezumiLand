using System.Collections;
using System.Collections.Generic;
using NL.EventContents;
using UnityEngine;
using System.Linq;

namespace NL {
    public class EventContentsExecuter {

        enum EventContentsExecuterState {
            ContentsPlaying,
            RewardReceive
        }

        private readonly IPlayerEventRepository playerEventRepository = null;
        private IEventContents currentEventContents = null;
        public IEventContents CurrentEventContents => currentEventContents;
        private EventContentsExecuterState currentEventContentsExecuterState = EventContentsExecuterState.ContentsPlaying;

        public EventContentsExecuter(IPlayerEventRepository playerEventRepository) {
            this.currentEventContents = new Invalid();
            this.playerEventRepository = playerEventRepository;
            this.currentEventContentsExecuterState = EventContentsExecuterState.ContentsPlaying;
        }

        /// <summary>
        /// 実行可能コンテンツを持っているかどうか？
        /// </summary>
        public bool HasPlayingContents => currentEventContents.EventContentsType != EventContentsType.InValid;

        /// <summary>
        /// コンテンツが実行中かどうか？
        /// </summary>
        public bool IsPlayingContentsFinish => !currentEventContents.IsAvilve();

        public void UpdateByFrame() {
            switch(currentEventContentsExecuterState) {
                case EventContentsExecuterState.ContentsPlaying : 
                {
                    Debug.Assert(currentEventContents != null, "currentEventContentsがありません");
                    if (this.HasPlayingContents) {
                        this.currentEventContents.OnUpdate();
                        if (this.IsPlayingContentsFinish) {
                            PlayNext();
                        }
                    }
                    break;
                }   
                case EventContentsExecuterState.RewardReceive : 
                {
                    break;
                }   
            }
        }

        public bool HasPlayableEvent() {
            return FetchPlayableEvent().Any();
        }

        public IEnumerable<PlayerEventModel> FetchPlayableEvent() {
            var clearEvents = this.playerEventRepository.GetClearEvent()
                .Where(eventModel => {
                    if (this.currentEventContents.TargetPlayerEventModel != null) {
                        // 実行中のイベントがある場合は実行中のイベントかどうかを判断
                        if (eventModel.Id == this.currentEventContents.TargetPlayerEventModel.Id) {
                            return false;
                        }
                    }
                    return true;
                });
            return clearEvents;
        }        

        /// <summary>
        /// 次の状態に進む
        /// - もしコンテンツ実行中の場合は進まない
        /// - 次のコンテンツがない場合は無効なコンテンツにする
        /// </summary>
        public void PlayNext() {

            if (!this.IsPlayingContentsFinish) {
                return;
            }

            if (!this.HasPlayableEvent()) {
                if (this.HasPlayingContents) {
                    PlayContents(new Invalid());
                }
                return;
            }

            var playableEvents = this.FetchPlayableEvent().ToList();
            this.PlayContents(EventContentsGenerator.Generate(playableEvents.First()));
        }

        public void PlayContents(IEventContents eventContentes) {
            Debug.Assert(this.currentEventContents != null, "currentEventContentsがありません");
            this.currentEventContents.OnExit();

            // 終わった段階で終了状態にする            
            if (this.currentEventContents.TargetPlayerEventModel != null) {
                this.currentEventContents.TargetPlayerEventModel.ToDone();
                playerEventRepository.Store(this.currentEventContents.TargetPlayerEventModel);
            }

            // 報酬がなければ次のイベントに移動する
            if (this.currentEventContents.TargetPlayerEventModel == null) {
                this.currentEventContents = eventContentes;
                eventContentes.OnEnter();
                return;
            }

            // 報酬の受け取り
            this.currentEventContentsExecuterState = EventContentsExecuterState.RewardReceive;
            var receiveReceiver = new RewardReceiver(this.currentEventContents.TargetPlayerEventModel);
            receiveReceiver.OnEndReceiveObservable.Subscribe(_ => {

                //報酬受取の終わったタイミングで次のイベントを起動する
                this.currentEventContentsExecuterState = EventContentsExecuterState.ContentsPlaying;
                this.currentEventContents = eventContentes;
                eventContentes.OnEnter();
            });            
            receiveReceiver.ReceiveRewardAndShowModel();            
        }
    }
}
