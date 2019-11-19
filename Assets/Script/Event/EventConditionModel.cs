using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public enum EventConditionType {
        None,
        Time,
        Satisfaction
    }

    public class EventConditionModel
    {
        public uint Id { get; private set; } 
        public EventConditionType EventConditionType { get; private set; }
        public string[] Arg { get; private set; }

        public EventConditionModel(
            uint id,
            EventConditionType eventConditionType,
            string[] arg
        )
        {
            this.Id = id;
            this.EventConditionType = eventConditionType;
            this.Arg = arg;
        }
    }
}