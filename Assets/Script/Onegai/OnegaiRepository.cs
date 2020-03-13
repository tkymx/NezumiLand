using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

namespace NL {

    [System.Serializable]
    public class OnegaiEntry : EntryBase {
        public string Title;
        public string Detail;
        public string Author;
        public string OnegaiType;
        public string OnegaiCondition;
        public string OnegaiConditionArg;
        public long Satisfaction;
        public bool IsInitialLock;
        public bool IsSchedule;
        public uint ScheduleId;
    }

    public interface IOnegaiRepository {
        IEnumerable<OnegaiModel> GetAll ();
        OnegaiModel Get (uint id);
    }

    public class OnegaiRepository : RepositoryBase<OnegaiEntry>, IOnegaiRepository {
        private readonly IScheduleRepository scheduleRepository = null;

        public OnegaiRepository (ContextMap contextMap, IScheduleRepository scheduleRepository) : base (contextMap.OnegaiEntrys) 
        {
            this.scheduleRepository = scheduleRepository;
        }

        public static OnegaiRepository GetRepository(ContextMap contextMap) 
        {
            var scheduleRepository = new ScheduleRepository(contextMap);
            return new OnegaiRepository(contextMap, scheduleRepository);
        }

        public IEnumerable<OnegaiModel> GetAll () {
            return entrys.Select (entry => {
                return CreateOnegaiModel(entry);
            });
        }

        public OnegaiModel Get (uint id) 
        {
            var entry = base.GetEntry(id);
            return CreateOnegaiModel(entry);
        }

        private OnegaiModel CreateOnegaiModel (OnegaiEntry entry) {

            ScheduleModel scheduleModel = null;
            if (entry.IsSchedule) {
                scheduleModel = scheduleRepository.Get(entry.ScheduleId);
                Debug.Assert(scheduleModel != null, "ScheduleModel がありません。");
            }

            return new OnegaiModel (
                entry.Id,
                entry.Title,
                entry.Detail,
                entry.Author,
                entry.OnegaiType,
                entry.OnegaiCondition,
                entry.OnegaiConditionArg,
                entry.Satisfaction,
                entry.IsInitialLock,
                scheduleModel);
        }
    }
}