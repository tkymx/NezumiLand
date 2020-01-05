using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL.EventCondition {

    public class InMonoSelectMode : IEventCondition {

        // 基本的には呼ばれたすべてを追加
        public InMonoSelectMode () {
        }

        public EventConditionType EventConditionType => EventConditionType.InMonoSelectMode;

        public List<EventConditionModel> Detect (List<EventConditionModel> eventConditionModels) {

            var outputEventConditionModels = new List<EventConditionModel> ();
            foreach (var eventConditionModel in eventConditionModels) {
                // 追加
                outputEventConditionModels.Add (eventConditionModel);
            }

            return outputEventConditionModels;
        }
    }
}