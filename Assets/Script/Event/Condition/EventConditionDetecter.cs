using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NL {
    public class EventConditionDetecter {
        private readonly IPlayerEventRepository playerEventRepository;

        public EventConditionDetecter (IPlayerEventRepository playerEventRepository) {
            this.playerEventRepository = playerEventRepository;
        }

        public void Detect (IEventCondition condition) {
            // 条件のクリア判定
            var targetPlayerEventModels = playerEventRepository.GetDetectable (condition.EventConditionType).ToList ();
            var updatePlayerEventModels = new List<PlayerEventModel> ();

            // 
            foreach (var targetPlayerEventModel in targetPlayerEventModels) {
                // 判断条件を集める（まだ判断が終わっていなくて、指定のタイプのもの）
                var targetEventConditionModels = targetPlayerEventModel
                    .Yets ()
                    .Where (model => model.EventConditionType == condition.EventConditionType)
                    .ToList ();

                // 条件を満たした判断条件リスト
                var detectedEventConditionModels = condition.Detect (targetEventConditionModels);

                // それぞれをDone にする
                detectedEventConditionModels.ForEach (model => targetPlayerEventModel.ToClear (model));

                // もし変更があれば更新する
                if (detectedEventConditionModels.Count > 0) {
                    updatePlayerEventModels.Add (targetPlayerEventModel);
                }
            }

            // クリアしたモデルを保存する
            foreach (var updatePlayerEventModel in updatePlayerEventModels) {
                playerEventRepository.Store (updatePlayerEventModel);
            }
        }
    }
}