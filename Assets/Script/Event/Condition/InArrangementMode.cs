using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL.EventCondition {

    public class InArrangementMode : IEventCondition {

        // 基本的には呼ばれたすべてを追加
        public InArrangementMode () {
        }

        public EventConditionType EventConditionType => EventConditionType.InArrangementMode;

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