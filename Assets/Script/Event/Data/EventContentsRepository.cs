using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

namespace NL {
    [DataContract]
    public class EventContentsEntry : EntryBase {

        [DataMember]
        public string EventContentsType { get; set; }

        [DataMember]
        public string[] Arg { get; set; }
    }

    public interface IEventContentsRepository {
        IEnumerable<EventContentsModel> GetAll ();
        EventContentsModel Get (uint id);
    }

    public class EventContentsRepository : RepositoryBase<EventContentsEntry>, IEventContentsRepository {
        public EventContentsRepository (ContextMap contextMap) : base (contextMap.EventContentsEntrys) { }

        public IEnumerable<EventContentsModel> GetAll () {
            return entrys.Select (entry => {
                return new EventContentsModel (
                    entry.Id,
                    parceEventContentsType (entry.EventContentsType),
                    entry.Arg);
            });
        }

        public EventContentsModel Get (uint id) {
            var entry = base.GetEntry(id);
            return new EventContentsModel (
                entry.Id,
                parceEventContentsType (entry.EventContentsType),
                entry.Arg);
        }

        private EventContentsType parceEventContentsType (string type) {
            if (Enum.TryParse (type, out EventContentsType outEventContentsType)) {
                return outEventContentsType;
            }
            return EventContentsType.InValid;
        }
    }
}