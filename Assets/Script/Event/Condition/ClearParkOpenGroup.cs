using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL.EventCondition {
    public struct ClearParkOpenGroupComparater {
        public uint ClearParkOpenGroupId { get; private set; }
        public ClearParkOpenGroupComparater (string[] args) {
            Debug.Assert (args[0] != "", "第一要素がありません");
            this.ClearParkOpenGroupId = uint.Parse (args[0]);
        }
        public bool IsClear(ParkOpenGroupModel parkOpenGroupModel) {
            return parkOpenGroupModel.Id == this.ClearParkOpenGroupId;
        }
    }

    /// <summary>
    /// お願いをクリアしたかどうか？
    /// </summary>
    public class ClearParkOpenGroup : IEventCondition {
        ParkOpenGroupModel ClearParkOpenGroupModel = null;

        public ClearParkOpenGroup (ParkOpenGroupModel ClearParkOpenGroupModel) {
            this.ClearParkOpenGroupModel = ClearParkOpenGroupModel;
        }

        public EventConditionType EventConditionType => EventConditionType.ClearParkOpenGroup;

        public List<EventConditionModel> Detect (List<EventConditionModel> eventConditionModels) {

            var outputEventConditionModels = new List<EventConditionModel> ();
            foreach (var eventConditionModel in eventConditionModels) {
                
                // 条件の取得
                var comparater = new ClearParkOpenGroupComparater (eventConditionModel.Arg);

                // お願いモデルのID
                if (!comparater.IsClear(ClearParkOpenGroupModel)) {
                    continue;
                }

                // 追加
                outputEventConditionModels.Add (eventConditionModel);
            }

            return outputEventConditionModels;
        }
    }
}