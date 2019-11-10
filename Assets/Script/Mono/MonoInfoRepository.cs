using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;

namespace NL
{
    [DataContract]
    public class MonoInfoEntry
    {
        [DataMember]
        public uint Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int Width { get; set; }

        [DataMember]
        public int Height { get; set; }

        [DataMember]
        public long MakingFee { get; set; }

        [DataMember]
        public long MakingItemAmount { get; set; }

        [DataMember]
        public long RemoveFee { get; set; }

        [DataMember]
        public string ModelName { get; set; }

        [DataMember]
        public long[] LevelUpFee { get; set; }

        [DataMember]
        public long[] LevelUpSatisfaction { get; set; }

        [DataMember]
        public long BaseSatisfaction { get; set; }
    }

    public interface IMonoInfoRepository
    {
        IEnumerable<MonoInfo> GetAll();
    }

    public class MonoInfoRepository : RepositoryBase<MonoInfoEntry>, IMonoInfoRepository
    {
        public MonoInfoRepository(ContextMap contextMap) : base(contextMap.MonoInfoEntrys)
        {
        }

        public IEnumerable<MonoInfo> GetAll()
        {
            return entrys.Select(entry =>
            {
                return new MonoInfo(
                    entry.Id,
                    entry.Name,
                    entry.Width,
                    entry.Height,
                    entry.MakingFee,
                    entry.MakingItemAmount,
                    entry.RemoveFee,
                    entry.ModelName,
                    entry.LevelUpFee,
                    entry.LevelUpSatisfaction,
                    entry.BaseSatisfaction);
            });
        }
    }
}
