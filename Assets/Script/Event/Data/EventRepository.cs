using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

namespace NL {
    [DataContract]
    public class EventEntry {
        [DataMember]
        public uint Id { get; set; }

        [DataMember]
        public uint[] EventConditionIds { get; set; }

        [DataMember]
        public uint EventContentsId { get; set; }

        [DataMember]
        public string EventRepeatType { get; set; }
    }

    public interface IEventRepository {
        IEnumerable<EventModel> GetAll ();
        EventModel Get (uint id);
    }

    public class EventRepository : RepositoryBase<EventEntry>, IEventRepository {
        private readonly IEventConditionRepository eventConditionRepository;
        private readonly IEventContentsRepository eventContentsRepository;

        public EventRepository (
            ContextMap contextMap,
            IEventConditionRepository eventConditionRepository,
            IEventContentsRepository eventContentsRepository) : base (contextMap.EventEntrys) {
            this.eventConditionRepository = eventConditionRepository;
            this.eventContentsRepository = eventContentsRepository;
        }

        public IEnumerable<EventModel> GetAll () {
            return entrys.Select (entry => {
                return new EventModel (
                    entry.Id,
                    entry.EventConditionIds.Select (id => this.eventConditionRepository.Get (id)).ToList(),
                    this.eventContentsRepository.Get (entry.EventContentsId),
                    this.parceEventRepeatType (entry.EventRepeatType));
            });
        }

        public EventModel Get (uint id) {
            var entry = this.entrys.Where (e => e.Id == id).First ();
            Debug.Assert (entry != null, "ファイルが見つかりません : " + id.ToString ());
            return new EventModel (
                entry.Id,
                entry.EventConditionIds.Select (eventConditionId => this.eventConditionRepository.Get (eventConditionId)).ToList (),
                this.eventContentsRepository.Get (entry.EventContentsId),
                this.parceEventRepeatType (entry.EventRepeatType));
        }

        private EventRepeatType parceEventRepeatType (string type) {
            if (Enum.TryParse (type, out EventRepeatType outEventRepeatType)) {
                return outEventRepeatType;
            }
            return EventRepeatType.None;
        }
    }
}