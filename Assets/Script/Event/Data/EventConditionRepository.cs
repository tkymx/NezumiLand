using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

namespace NL {
    [DataContract]
    public class EventConditionEntry : EntryBase {

        [DataMember]
        public string EventConditionType { get; set; }

        [DataMember]
        public string[] Arg { get; set; }
    }

    public interface IEventConditionRepository {
        IEnumerable<EventConditionModel> GetAll ();
        EventConditionModel Get (uint id);
    }

    public class EventConditionRepository : RepositoryBase<EventConditionEntry>, IEventConditionRepository {
        public EventConditionRepository (ContextMap contextMap) : base (contextMap.EventConditionEntrys) { }

        public IEnumerable<EventConditionModel> GetAll () {
            return entrys.Select (entry => {
                return new EventConditionModel (
                    entry.Id,
                    parceEventConditionType (entry.EventConditionType),
                    entry.Arg);
            });
        }

        public EventConditionModel Get (uint id) {
            var entry = base.GetEntry(id);
            return new EventConditionModel (
                entry.Id,
                parceEventConditionType (entry.EventConditionType),
                entry.Arg);
        }

        private EventConditionType parceEventConditionType (string type) {
            if (Enum.TryParse (type, out EventConditionType outEventConditionType)) {
                return outEventConditionType;
            }
            return EventConditionType.None;
        }
    }
}