using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NL {
    public class OnegaiMediater {
        private readonly IPlayerOnegaiRepository playerOnegaiRepository;

        public OnegaiMediater (IPlayerOnegaiRepository playerOnegaiRepository) {
            this.playerOnegaiRepository = playerOnegaiRepository;
        }
        
        /// <summary>
        /// 未クリアのお願いがクリアかを判断する
        /// </summary>
        /// <param name="conditionBase"></param>
        /// <param name="targetPlayerOnegaiModels"></param>
        public void Mediate (IOnegaiConditionBase conditionBase, List<PlayerOnegaiModel> targetPlayerOnegaiModels) {

            var mediatablePlayerOnegaiModel = targetPlayerOnegaiModels                
                .Where (model => model.OnegaiState == OnegaiState.UnLock)
                .Where (model => model.OnegaiModel.OnegaiCondition == conditionBase.OnegaiCondition);

            var clearPlayerOnegaiModels = conditionBase.Mediate (targetPlayerOnegaiModels);

            // クリアしたモデルを保存する
            foreach (var clearPlayerOnegaiModel in clearPlayerOnegaiModels) {
                playerOnegaiRepository.Store (clearPlayerOnegaiModel);
            }
        }

        /// <summary>
        /// すべてのお願いをリセットして、再度クリアかを判断する
        /// 条件が今も満たされているかをこれで判断する意図
        /// </summary>
        /// <param name="conditionBase"></param>
        /// <param name="targetPlayerOnegaiModels"></param>
        public void ResetAndMediate (IOnegaiConditionBase conditionBase, List<PlayerOnegaiModel> targetPlayerOnegaiModels) {

            // クリア状況をリセットするモデルを保存する
            foreach (var targetPlayerOnegaiModel in targetPlayerOnegaiModels) {
                targetPlayerOnegaiModel.Reset();
            }

            // クリア状況を更新する
            conditionBase.Mediate (targetPlayerOnegaiModels);

            // 情報を保存する
            foreach (var targetPlayerOnegaiModel in targetPlayerOnegaiModels) {
                playerOnegaiRepository.Store (targetPlayerOnegaiModel);
            }
        }

    }
}