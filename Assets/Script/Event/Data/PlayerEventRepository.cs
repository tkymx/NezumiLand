using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

namespace NL {
    [DataContract]
    public class PlayerEventEntry : EntryBase{
        [DataMember]
        public uint EventId { get; set; }

        [DataMember]
        public string EventState { get; set; }

        [DataMember]
        public uint[] doneEventConditionIds { get; set; }
    }

    public interface IPlayerEventRepository {
        IEnumerable<PlayerEventModel> GetDetectable (EventConditionType eventConditionType);
        IEnumerable<PlayerEventModel> GetAll ();
        PlayerEventModel Get (uint id);
        IEnumerable<PlayerEventModel> GetClearEvent ();
        void Store (PlayerEventModel playerEventModel);
    }

    /// <summary>
    /// プレイヤーのお願いの状態を持っている
    /// 最終的には、持つだけではなく、保存まで行いたいが、現状は仕組みができていないので一時的な情報のみを保持する
    /// </summary>
    public class PlayerEventRepository : PlayerRepositoryBase<PlayerEventEntry>, IPlayerEventRepository {
        private readonly IEventRepository eventRepository;
        private readonly IEventConditionRepository eventConditionRepository;

        public PlayerEventRepository (IEventRepository eventRepository, IEventConditionRepository eventConditionRepository, PlayerContextMap playerContextMap) : base (playerContextMap.PlayerEventEntrys) {
            this.eventRepository = eventRepository;
            this.eventConditionRepository = eventConditionRepository;

            // 現在あるイベントで登録されていないものを登録 TODO: 後で外だし。
            var models = this.eventRepository.GetAll ().ToList();
            foreach (var eventModel in models) {
                if (this.entrys.Any (entry => entry.EventId == eventModel.Id)) {
                    continue;
                }

                this.entrys.Add (new PlayerEventEntry () {
                    Id = eventModel.Id,
                        EventId = eventModel.Id,
                        EventState = EventState.UnLock.ToString (),
                        doneEventConditionIds = new uint[0]
                });
            }
        }

        public static PlayerEventRepository GetRepository (ContextMap contextMap, PlayerContextMap playerContextMap) {
            IEventConditionRepository eventConditionRepository = new EventConditionRepository (contextMap);
            IEventContentsRepository eventContentsRepository = new EventContentsRepository (contextMap);
            IRewardRepository rewardRepository = new RewardRepository(contextMap);
            IEventRepository eventRepository = new EventRepository (contextMap, eventConditionRepository, eventContentsRepository, rewardRepository);
            return new PlayerEventRepository (eventRepository, eventConditionRepository, playerContextMap);
        }

        public IEnumerable<PlayerEventModel> GetAll () {
            return this.entrys
                .Select (entry => new PlayerEventModel (
                    entry.Id,
                    eventRepository.Get (entry.Id),
                    entry.EventState.ToString (),
                    entry.doneEventConditionIds.Select (eventConditionId => eventConditionRepository.Get (eventConditionId)).ToList ()));
        }

        public IEnumerable<PlayerEventModel> GetDetectable (EventConditionType eventConditionType) {
            return GetAll ()
                .Where (model => model.EventState == EventState.UnLock)
                .Where (model => model.HasDetectableCondition (eventConditionType));
        }

        public IEnumerable<PlayerEventModel> GetClearEvent () {
            return GetAll()
                .Where (model => model.EventState == EventState.Clear);        
        }

        public PlayerEventModel Get (uint id) {
            var entry = this.GetEntry(id);
            if (entry == null) {
                var eventModel = eventRepository.Get (id);
                Debug.Assert(eventModel != null, "イベントのモデルが存在しません : " + id);

                return new PlayerEventModel (
                    id,
                    eventModel,
                    EventState.Lock.ToString(),
                    new List<EventConditionModel>());
            }
            return new PlayerEventModel (
                entry.Id,
                eventRepository.Get (entry.Id),
                entry.EventState.ToString (),
                entry.doneEventConditionIds.Select (eventConditionId => eventConditionRepository.Get (eventConditionId)).ToList ());
        }

        public void Store (PlayerEventModel playerEventModel) {
            var entry = this.entrys.Where (e => e.Id == playerEventModel.Id).First ();
            Debug.Assert (entry != null, "保存対象が見つかりませんでした。");
            var index = this.entrys.IndexOf (entry);
            this.entrys[index] = new PlayerEventEntry () {
                Id = playerEventModel.Id,
                EventId = playerEventModel.EventModel.Id,
                EventState = playerEventModel.EventState.ToString (),
                doneEventConditionIds = playerEventModel.doneEventConditionModels.Select(model => model.Id).ToArray()
            };
            PlayerContextMap.WriteEntry (this.entrys);
        }
    }
}