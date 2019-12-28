using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

namespace NL {

    [System.Serializable]
    public class ScheduleEntry : EntryBase {

        
        public float CloseElapsedTime;
    }

    public interface IScheduleRepository {
        IEnumerable<ScheduleModel> GetAll ();
        ScheduleModel Get (uint id);
    }

    public class ScheduleRepository : RepositoryBase<ScheduleEntry>, IScheduleRepository {
        public ScheduleRepository (ContextMap contextMap) : base (contextMap.ScheduleEntrys) { }

        public IEnumerable<ScheduleModel> GetAll () {
            return entrys.Select (entry => {
                return new ScheduleModel (
                    entry.Id,
                    entry.CloseElapsedTime);
            });
        }

        public ScheduleModel Get (uint id) 
        {
            var entry = base.GetEntry(id);
            return new ScheduleModel (
                entry.Id,
                entry.CloseElapsedTime);
        }
    }
}