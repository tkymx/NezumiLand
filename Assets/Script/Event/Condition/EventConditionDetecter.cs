using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace NL 
{
    public class EventConditionDetecter
    {
        private readonly IPlayerEventRepository playerEventRepository;

        public EventConditionDetecter(IPlayerEventRepository playerEventRepository)
        {
            this.playerEventRepository = playerEventRepository;            
        }

        public void Detect(IEventCondtion condition) 
        {
            // 条件のクリア判定
            var targetPlayerEventModels = playerEventRepository.GetDetectable(condition.EventConditionType).ToList();
            var updatePlayerEventModels = condition.Detect(targetPlayerEventModels);

            // クリアしたモデルを保存する
            foreach (var updatePlayerEventModel in updatePlayerEventModels)
            {
                playerEventRepository.Store(updatePlayerEventModel);
            }
        }
    }
}

