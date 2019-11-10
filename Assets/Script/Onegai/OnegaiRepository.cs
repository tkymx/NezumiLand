using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;

namespace NL
{
    [DataContract]
    public class OnegaiEntry
    {
        [DataMember]
        public uint Id { get; set; }

        [DataMember]
        public uint TriggerMonoInfoId { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Detail { get; set; }

        [DataMember]
        public string OnegaiCondition { get; set; }

        [DataMember]
        public string OnegaiConditionArg { get; set; }
    }

    public interface IOnegaiRepository
    {
        IEnumerable<OnegaiModel> GetAll();
    }

    public class OnegaiRepository : RepositoryBase<OnegaiEntry>, IOnegaiRepository
    {
        public OnegaiRepository(ContextMap contextMap) : base(contextMap.OnegaiEntrys)
        {
        }

        public IEnumerable<OnegaiModel> GetAll()
        {
            return entrys.Select(entry =>
            {
                return new OnegaiModel(
                    entry.Id,
                    entry.TriggerMonoInfoId,
                    entry.Title,
                    entry.Detail,
                    entry.OnegaiCondition,
                    entry.OnegaiConditionArg);
            });
        }
    }
}
