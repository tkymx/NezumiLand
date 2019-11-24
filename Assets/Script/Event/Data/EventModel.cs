using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public enum EventRepeatType {
        None,
        Once,
        Allways
    }

    public class EventModel {
        public uint Id { get; private set; }
        public List<EventConditionModel> EventConditionModels { get; private set; }
        public EventContentsModel EventContentsModel { get; private set; }
        public EventRepeatType EventRepeatType { get; private set; }

        public EventModel (
            uint id,
            List<EventConditionModel> eventConditionModels,
            EventContentsModel eventContentsModel,
            EventRepeatType eventRepeatType
        ) {
            this.Id = id;
            this.EventConditionModels = eventConditionModels;
            this.EventContentsModel = eventContentsModel;
            this.EventRepeatType = eventRepeatType;
        }
    }
}