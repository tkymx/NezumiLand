using System;
using System.Linq;
using UnityEngine;

namespace NL
{
    class EventUnLockService    
    {
        private readonly IEventRepository eventRepository = null;
        private readonly IPlayerEventRepository playerEventRepository = null;

        public EventUnLockService(IEventRepository eventRepository, IPlayerEventRepository playerEventRepository)
        {
            this.eventRepository = eventRepository;
            this.playerEventRepository = playerEventRepository;
        }

        public void Execute() { 

            // 現在あるイベントで登録されていないものを登録 TODO: 後で外だし。
            var models = this.eventRepository.GetAll ().ToList();
            foreach (var eventModel in models) {
                var playerEventModel = this.playerEventRepository.Get(eventModel.Id);
                Debug.Assert(playerEventModel != null, "イベントが取れませんでした。。。");

                if (!playerEventModel.IsLock()) {
                    continue;
                }

                // UnLock 状態にする
                playerEventModel.ToUnLock();

                // 保存する
                playerEventRepository.Store(playerEventModel);
            }
        }
    }
    
}