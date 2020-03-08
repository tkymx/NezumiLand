using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public enum EventConditionType {
        None,
        Time,
        AboveSatisfaction,
        ClearOnegai,
        InMonoSelectMode,
        InArrangementMode,
        AfterArrangement,
        ClearParkOpenGroup
    }

    public class EventConditionModel: ModelBase {
        
        public EventConditionType EventConditionType { get; private set; }
        public string[] Arg { get; private set; }

        public EventConditionModel (
            uint id,
            EventConditionType eventConditionType,
            string[] arg
        ) {
            this.Id = id;
            this.EventConditionType = eventConditionType;
            this.Arg = arg;
        }
    }
}