using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

namespace NL {

    [System.Serializable]
    public class ParkOpenGroupEntry : EntryBase {
        public uint[] ParkOpenWaveIds;
        public int MaxHeartCount;
        public int GoalHeartCount;
        public uint ClearRewardId;
        public uint FirstClearRewardId;
        public uint SpecialClearRewardId;
        public int[] ObtainHeartCounts;
        public uint[] ObtainHeartRewardIds;
        // View
        public string ViewGroupName;
        public string ViewGroupDescription;
        public Position2Entry ViewSelectorPosition;
        public string ViewIconName;
    }

    public interface IParkOpenGroupRepository {
        IEnumerable<ParkOpenGroupModel> GetAll ();
        ParkOpenGroupModel Get (uint id);
    }

    public class ParkOpenGroupRepository : RepositoryBase<ParkOpenGroupEntry>, IParkOpenGroupRepository {

        private IParkOpenWaveRepository parkOpenWaveRepository;
        private IRewardRepository rewardRepository;

        public ParkOpenGroupRepository (IParkOpenWaveRepository parkOpenWaveRepository, IRewardRepository rewardRepository, ContextMap contextMap) : base (contextMap.ParkOpenGroupEntrys) {
            this.parkOpenWaveRepository = parkOpenWaveRepository;
            this.rewardRepository = rewardRepository;
        }

        public ParkOpenGroupModel CreateFromEntry (ParkOpenGroupEntry entry)
        {
            var parkOpenWaveModels = entry.ParkOpenWaveIds.Select(id => {
                var parkOpenWaveModel = this.parkOpenWaveRepository.Get(id);
                Debug.Assert(parkOpenWaveModel != null, "parkOpenWaveModelがありません。 " + id.ToString());
                return parkOpenWaveModel;
            }); 

            var clearRewardModel = this.rewardRepository.Get(entry.ClearRewardId);
            Debug.Assert(clearRewardModel != null, "報酬がありません ID = " + entry.ClearRewardId.ToString());

            var firstClearRewardModel = this.rewardRepository.Get(entry.FirstClearRewardId);
            Debug.Assert(firstClearRewardModel != null, "報酬がありません ID = " + entry.FirstClearRewardId.ToString());

            var specialClearRewardModel = this.rewardRepository.Get(entry.SpecialClearRewardId);
            Debug.Assert(specialClearRewardModel != null, "報酬がありません ID = " + entry.SpecialClearRewardId.ToString());

            var obtainedHeartRewardAmounts = new List<ParkOpenHeartRewardAmount>();
            for (int rewardIndex = 0; rewardIndex < entry.ObtainHeartCounts.Count(); rewardIndex++)
            {
                var heartCount = entry.ObtainHeartCounts[rewardIndex];
                var rewardId = entry.ObtainHeartRewardIds[rewardIndex];

                var obtainHeartRewardModel = this.rewardRepository.Get(rewardId);
                Debug.Assert(obtainHeartRewardModel != null, "報酬がありません ID = " + rewardId.ToString());

                obtainedHeartRewardAmounts.Add(new ParkOpenHeartRewardAmount(heartCount,obtainHeartRewardModel));
            }

            return new ParkOpenGroupModel (
                entry.Id,
                parkOpenWaveModels.ToArray(),
                entry.MaxHeartCount,
                entry.GoalHeartCount,
                new ParkOpenGroupViewInfo(
                    entry.ViewGroupName,
                    entry.ViewGroupDescription,
                    entry.ViewSelectorPosition.ToVector2(),
                    entry.ViewIconName
                ),
                clearRewardModel,
                firstClearRewardModel,
                specialClearRewardModel,
                obtainedHeartRewardAmounts);
        }

        public IEnumerable<ParkOpenGroupModel> GetAll () {
            return entrys.Select (entry => {
                return Get(entry.Id);
            });
        }

        public ParkOpenGroupModel Get (uint id) 
        {
            var entry = base.GetEntry(id);
            Debug.Assert(entry != null, id + "が見つかりません");
            return this.CreateFromEntry(entry);
        }
    }
}