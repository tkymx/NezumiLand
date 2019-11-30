using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL.EventCondition {
    public struct ClearOnegaiComparater {
        public uint ClearOnegaiId { get; private set; }
        public ClearOnegaiComparater (string[] args) {
            Debug.Assert (args[0] != "", "第一要素がありません");
            this.ClearOnegaiId = uint.Parse (args[0]);
        }
        public bool IsClear(OnegaiModel onegaiModel) {
            return onegaiModel.Id == this.ClearOnegaiId;
        }
    }

    /// <summary>
    /// お願いをクリアしたかどうか？
    /// </summary>
    public class ClearOnegai : IEventCondition {
        OnegaiModel clearOnegaiModel = null;

        public ClearOnegai (OnegaiModel clearOnegaiModel) {
            this.clearOnegaiModel = clearOnegaiModel;
        }

        public EventConditionType EventConditionType => EventConditionType.ClearOnegai;

        public List<EventConditionModel> Detect (List<EventConditionModel> eventConditionModels) {

            var outputEventConditionModels = new List<EventConditionModel> ();
            foreach (var eventConditionModel in eventConditionModels) {
                // 条件の取得
                var comparater = new ClearOnegaiComparater (eventConditionModel.Arg);

                // お願いモデルのID
                if (!comparater.IsClear(clearOnegaiModel)) {
                    continue;
                }

                // 追加
                outputEventConditionModels.Add (eventConditionModel);
            }

            return outputEventConditionModels;
        }
    }
}