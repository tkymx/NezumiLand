using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace NL {
    [System.Serializable]
    public class MonoInfoEntry : EntryBase {
        public string Name;
        public string Type;
        public int Width;
        public int Height;
        public long MakingFee;
        public long MakingItemAmount;
        public long RemoveFee;
        public string ModelName;
        public long[] LevelUpFee;
        public long[] LevelUpSatisfaction;
        public long BaseSatisfaction;
        public long ArrangementCount;
        public string ReleaseConditionText;
        public float MakingTime;
        public int PromotionCount;
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
                entry.MakingTime,
                entry.PromotionCount);
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