using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using UnityEngine;
using System;

namespace NL
{
    [DataContract]
    public class EventConditionEntry
    {
        [DataMember]
        public uint Id { get; set; }

        [DataMember]
        public string EventConditionType { get; set; }

        [DataMember]
        public string[] Arg { get; set; }
    }

    public interface IEventConditionRepository
    {
        IEnumerable<EventConditionModel> GetAll();
        EventConditionModel Get(uint id);        
    }

    public class EventConditionRepository : RepositoryBase<EventConditionEntry>, IEventConditionRepository
    {
        public EventConditionRepository(ContextMap contextMap) : base(contextMap.EventConditionEntrys)
        {
        }

        public IEnumerable<EventConditionModel> GetAll()
        {
            return entrys.Select(entry =>
            {
                return new EventConditionModel(
                    entry.Id,
                    parceEventConditionType(entry.EventConditionType),
                    entry.Arg);
            });
        }

        public EventConditionModel Get(uint id)
        {
            var entry = this.entrys.Where(e => e.Id == id).First();
            Debug.Assert(entry != null, "ファイルが見つかりません : " + id.ToString());
            return new EventConditionModel(
                entry.Id,
                parceEventConditionType(entry.EventConditionType),
                entry.Arg);
        }

        private EventConditionType parceEventConditionType(string type) {
            if (Enum.TryParse(type, out EventConditionType outEventConditionType))
            {
                return outEventConditionType;
            }
            return EventConditionType.None;
        }
    }
}
