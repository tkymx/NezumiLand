using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace NL {
    [DataContract]
    public class MonoInfoEntry : EntryBase {

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Type { get; set; }

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

        [DataMember]
        public long ArrangementCount { get; set; }

        [DataMember]
        public string ReleaseConditionText { get; set; }

        [DataMember]
        public float MakingTime { get; set; }
    }

    public interface IMonoInfoRepository {
        IEnumerable<MonoInfo> GetAll ();
        MonoInfo Get (uint id);
        IEnumerable<MonoInfo> Gets (List<uint> ids);
        IEnumerable<MonoInfo> GetByType (MonoType type);
        ArrangementMaxCount GetMaxCount (List<uint> ids);
    }

    public class MonoInfoRepository : RepositoryBase<MonoInfoEntry>, IMonoInfoRepository {
        public MonoInfoRepository (ContextMap contextMap) : base (contextMap.MonoInfoEntrys) { }

        public IEnumerable<MonoInfo> GetAll () {
            return entrys.Select (entry => {
                return this.CreateFromEntry(entry);
            });
        }

        public MonoInfo Get(uint id) {
            var entry = this.GetEntry(id);
            return this.CreateFromEntry(entry);
        }

        public IEnumerable<MonoInfo> Gets (List<uint> ids) {
            return ids.Select(id => Get(id));
        }

        private MonoInfo CreateFromEntry (MonoInfoEntry entry) {
            return new MonoInfo (
                entry.Id,
                entry.Name,
                entry.Type,
                entry.Width,
                entry.Height,
                entry.MakingFee,
                entry.MakingItemAmount,
                entry.RemoveFee,
                entry.ModelName,
                entry.LevelUpFee,
                entry.LevelUpSatisfaction,
                entry.BaseSatisfaction,
                entry.ArrangementCount,
                entry.ReleaseConditionText,
                entry.MakingTime);
        }

        public IEnumerable<MonoInfo> GetByType (MonoType type) {
            return GetAll ()
                .Where (monoInfo => monoInfo.Type == type);
        }

        public ArrangementMaxCount GetMaxCount (List<uint> ids) {
            var arrangementMaxCount = new ArrangementMaxCount();
            foreach (var model in Gets(ids))
            {
                arrangementMaxCount += model.ArrangementMaxCount;   
            }
            return arrangementMaxCount;
        }
    }
}