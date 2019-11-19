using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace NL 
{
    public class OnegaiMediater
    {
        private readonly IPlayerOnegaiRepository playerOnegaiRepository;

        public OnegaiMediater(IPlayerOnegaiRepository playerOnegaiRepository)
        {
            this.playerOnegaiRepository = playerOnegaiRepository;            
        }

        public void Mediate(IOnegaiConditionBase conditionBase, uint targetMonoInfoId) 
        {
            // 条件のクリア判定
            var targetPlayerOnegaiModels = playerOnegaiRepository.GetMediatable(conditionBase.OnegaiCondition, targetMonoInfoId).ToList();
            var clearPlayerOnegaiModels = conditionBase.Mediate(targetPlayerOnegaiModels);

            // クリアしたモデルを保存する
            foreach (var clearPlayerOnegaiModel in clearPlayerOnegaiModels)
            {
                playerOnegaiRepository.Store(clearPlayerOnegaiModel);
            }
        }
    }
}

