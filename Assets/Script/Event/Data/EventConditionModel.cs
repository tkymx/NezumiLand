using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public enum EventConditionType {
        None,
        Time,
        AboveSatisfaction
    }

    public class EventConditionModel
    {
        public uint Id { get; private set; } 
        public EventConditionType EventConditionType { get; private set; }
        public string[] Arg { get; private set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            var value = obj as EventConditionModel;
            return Id.Equals(value.Id);                        
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

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