using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public enum EventRepeatType {
        None,
        Once,
        Allways
    }

    public class EventModel
    {
        public uint Id { get; private set; } 
        public EventConditionModel[] EventConditionModels { get; private set; }
        public EventContentsModel EventContentsModel { get; private set; }
        public EventRepeatType EventRepeatType { get; private set; }

        public EventModel(
            uint id,
            EventConditionModel[] eventConditionModels,
            EventContentsModel eventContentsModel,
            EventRepeatType eventRepeatType
        )
        {
            this.Id = id;
            this.EventConditionModels = EventConditionModels;
            this.EventContentsModel = eventContentsModel;
            this.EventRepeatType = eventRepeatType;            
        }
    }
}