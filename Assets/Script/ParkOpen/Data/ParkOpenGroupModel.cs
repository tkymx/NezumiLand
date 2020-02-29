using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    // 見た目の情報
    public struct ParkOpenGroupViewInfo
    {
        public string GroupName { get; private set; }
        public string GroupDescription { get; private set; }
        public Vector2 SelectorPosition { get; private set; }
        public string IconName { get; private set; }

        public ParkOpenGroupViewInfo(
            string groupName,
            string groupDescription,
            Vector2 selectorPosition,
            string iconName)
        {
            this.GroupName = groupName;
            this.GroupDescription = groupDescription;
            this.SelectorPosition = selectorPosition;
            this.IconName = iconName;
        }
    }

    /// <summary>
    /// 遊園地公開時のキャラクタ出現のウェーブ
    /// </summary>
    public class ParkOpenGroupModel : ModelBase
    {
        public ParkOpenWaveModel[] ParkOpenWaves { get; private set; }
        public int MaxHeartCount { get; private set; }
        public int GoalHeartCount { get; private set; }
        public ParkOpenGroupViewInfo ParkOpenGroupViewInfo { get; private set; }
        public RewardModel ClearReward { get; private set; }

        public ParkOpenGroupModel(
            uint id,
            ParkOpenWaveModel[] parkOpenWaves,
            int maxHeartCount,
            int goalHeartCount,
            ParkOpenGroupViewInfo parkOpenGroupViewInfo,
            RewardModel clearReward
        )
        {
            this.Id = id;
            this.ParkOpenWaves = parkOpenWaves;
            this.MaxHeartCount = maxHeartCount;
            this.GoalHeartCount = goalHeartCount;
            this.ParkOpenGroupViewInfo = parkOpenGroupViewInfo;
            this.ClearReward = clearReward;
        }
    }
}

