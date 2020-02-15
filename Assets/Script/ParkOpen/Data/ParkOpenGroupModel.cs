using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    // 見た目の情報
    public struct ParkOpenGroupViewInfo
    {
        public string GroupName { get; private set; }
        public Vector2 SelectorPosition { get; private set; }
        public string IconName { get; private set; }

        public ParkOpenGroupViewInfo(
            string groupName,
            Vector2 selectorPosition,
            string iconName)
        {
            this.GroupName = groupName;
            this.SelectorPosition = selectorPosition;
            this.IconName = iconName;
        }
    }

    // 遊び場公開の報酬情報
    public struct ParkOpenGroupReward
    {
        public Currency Currency { get; private set; }
        public ArrangementItemAmount ArrangementItemAmount { get; private set; }

        public ParkOpenGroupReward(
            Currency currency,
            ArrangementItemAmount arrangementItemAmount)
        {
            this.Currency = currency;
            this.ArrangementItemAmount = arrangementItemAmount;
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
        public ParkOpenGroupReward ParkOpenGroupReward { get; private set; }

        public ParkOpenGroupModel(
            uint id,
            ParkOpenWaveModel[] parkOpenWaves,
            int maxHeartCount,
            int goalHeartCount,
            ParkOpenGroupViewInfo parkOpenGroupViewInfo,
            ParkOpenGroupReward parkOpenGroupReward
        )
        {
            this.Id = id;
            this.ParkOpenWaves = parkOpenWaves;
            this.MaxHeartCount = maxHeartCount;
            this.GoalHeartCount = goalHeartCount;
            this.ParkOpenGroupViewInfo = parkOpenGroupViewInfo;
            this.ParkOpenGroupReward = parkOpenGroupReward;
        }
    }
}

