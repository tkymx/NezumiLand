using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

namespace NL {
    [DataContract]
    public class OnegaiEntry : EntryBase {

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Detail { get; set; }

        [DataMember]
        public string Author { get; set; }

        [DataMember]
        public string OnegaiCondition { get; set; }

        [DataMember]
        public string OnegaiConditionArg { get; set; }

        [DataMember]
        public long Satisfaction { get; set; }

        [DataMember]
        public bool IsInitialLock { get; set; }
    }

    public interface IOnegaiRepository {
        IEnumerable<OnegaiModel> GetAll ();
        OnegaiModel Get (uint id);
    }

    public class OnegaiRepository : RepositoryBase<OnegaiEntry>, IOnegaiRepository {
        public OnegaiRepository (ContextMap contextMap) : base (contextMap.OnegaiEntrys) { }

        public IEnumerable<OnegaiModel> GetAll () {
            return entrys.Select (entry => {
                return new OnegaiModel (
                    entry.Id,
                    entry.Title,
                    entry.Detail,
                    entry.Author,
                    entry.OnegaiCondition,
                    entry.OnegaiConditionArg,
                    entry.Satisfaction,
                    entry.IsInitialLock);
            });
        }

        public OnegaiModel Get (uint id) 
        {
            var entry = base.GetEntry(id);
            return new OnegaiModel (
                entry.Id,
                entry.Title,
                entry.Detail,
                entry.Author,
                entry.OnegaiCondition,
                entry.OnegaiConditionArg,
                entry.Satisfaction,
                entry.IsInitialLock);
        }
    }
}