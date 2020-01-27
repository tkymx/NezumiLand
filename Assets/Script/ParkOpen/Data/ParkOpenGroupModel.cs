using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    /// <summary>
    /// 遊園地公開時のキャラクタ出現のウェーブ
    /// </summary>
    public class ParkOpenGroupModel : ModelBase
    {
        public ParkOpenWaveModel[] ParkOpenWaves { get; private set; }
        public int MaxHeartCount { get; private set; }
        public int GoalHeartCOunt { get; private set; }

        public ParkOpenGroupModel(
            uint id,
            ParkOpenWaveModel[] parkOpenWaves,
            int maxHeartCount,
            int gloalHeartCount
        )
        {
            this.Id = id;
            this.ParkOpenWaves = parkOpenWaves;
            this.MaxHeartCount = maxHeartCount;
            this.GoalHeartCOunt = GoalHeartCOunt;
        }
    }
}

