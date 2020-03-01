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
        public int[] SpecialClearRewardObtainHearts;
        public uint[] SpecialClearRewardIds;
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

            var specialClearRewardAmounts = new List<ParkOpenRewardAmount>();
            for (int rewardIndex = 0; rewardIndex < entry.SpecialClearRewardIds.Count(); rewardIndex++)
            {
                var heartCount = entry.SpecialClearRewardObtainHearts[rewardIndex];
                var rewardId = entry.SpecialClearRewardIds[rewardIndex];

                var specialClearRewardModel = this.rewardRepository.Get(rewardId);
                Debug.Assert(specialClearRewardModel != null, "報酬がありません ID = " + rewardId.ToString());

                specialClearRewardAmounts.Add(new ParkOpenRewardAmount(heartCount,specialClearRewardModel));
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
                specialClearRewardAmounts);
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