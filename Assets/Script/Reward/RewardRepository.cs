using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

namespace NL {

    [DataContract]
    public class RewardSingleEntry {
        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public uint Amount { get; set; }

        [DataMember]
        public string[] Args { get; set; }
    }
    
    [DataContract]
    public class RewardEntry {
        [DataMember]
        public uint Id { get; set; }

        [DataMember]
        public RewardSingleEntry[] RewardSingleEntrys { get; set; }
    }

    public interface IRewardRepository {
        IEnumerable<RewardModel> GetAll ();
        RewardModel Get (uint id);
    }

    public class RewardRepository : RepositoryBase<RewardEntry>, IRewardRepository {
        public RewardRepository (ContextMap contextMap) : base (contextMap.RewardEntrys) { }

        public IEnumerable<RewardModel> GetAll () {
            return entrys.Select (entry => {
                return new RewardModel (
                    entry.Id,
                    Generate(entry.RewardSingleEntrys));
            });
        }

        public RewardModel Get (uint id) {
            var entry = this.entrys.Where (e => e.Id == id).First ();
            Debug.Assert (entry != null, "ファイルが見つかりません : " + id.ToString ());
            return new RewardModel (
                    entry.Id,
                    Generate(entry.RewardSingleEntrys));
        }

        private List<IRewardAmount> Generate(RewardSingleEntry[] rewards) {
            return rewards
                .Select(reward => RewardGenerator.Generate(reward.Type, reward.Amount, reward.Args))
                .ToList();
        }
    }
}
