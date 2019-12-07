using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

namespace NL {
    [DataContract]
    public class EventEntry : EntryBase {

        [DataMember]
        public uint[] EventConditionIds { get; set; }

        [DataMember]
        public uint EventContentsId { get; set; }

        [DataMember]
        public string EventRepeatType { get; set; }

        [DataMember]
        public uint RewardId { get; set; }
    }

    public interface IEventRepository {
        IEnumerable<EventModel> GetAll ();
        EventModel Get (uint id);
    }

    public class EventRepository : RepositoryBase<EventEntry>, IEventRepository {
        private readonly IEventConditionRepository eventConditionRepository;
        private readonly IEventContentsRepository eventContentsRepository;
        private readonly IRewardRepository rewardRepository;

        public EventRepository (
            ContextMap contextMap,
            IEventConditionRepository eventConditionRepository,
            IEventContentsRepository eventContentsRepository,
            IRewardRepository rewardRepository) : base (contextMap.EventEntrys) 
        {
            this.eventConditionRepository = eventConditionRepository;
            this.eventContentsRepository = eventContentsRepository;
            this.rewardRepository = rewardRepository;
        }

        public static EventRepository GetRepository (ContextMap contextMap) {
            IEventConditionRepository eventConditionRepository = new EventConditionRepository (contextMap);
            IEventContentsRepository eventContentsRepository = new EventContentsRepository (contextMap);
            IRewardRepository rewardRepository = new RewardRepository(contextMap);
            return new EventRepository (contextMap, eventConditionRepository, eventContentsRepository, rewardRepository);
        }

        public IEnumerable<EventModel> GetAll () {
            return entrys.Select (entry => {
                return new EventModel (
                    entry.Id,
                    entry.EventConditionIds.Select (id => this.eventConditionRepository.Get (id)).ToList(),
                    this.eventContentsRepository.Get (entry.EventContentsId),
                    this.parceEventRepeatType (entry.EventRepeatType),
                    this.rewardRepository.Get(entry.RewardId));
            });
        }

        public EventModel Get (uint id) {
            var entry = base.GetEntry(id);
            return new EventModel (
                entry.Id,
                entry.EventConditionIds.Select (eventConditionId => this.eventConditionRepository.Get (eventConditionId)).ToList (),
                this.eventContentsRepository.Get (entry.EventContentsId),
                this.parceEventRepeatType (entry.EventRepeatType),
                this.rewardRepository.Get(entry.RewardId));
        }

        private EventRepeatType parceEventRepeatType (string type) {
            if (Enum.TryParse (type, out EventRepeatType outEventRepeatType)) {
                return outEventRepeatType;
            }
            return EventRepeatType.None;
        }
    }
}