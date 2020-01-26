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

        public ParkOpenGroupModel(
            uint id,
            ParkOpenWaveModel[] parkOpenWaves
        )
        {
            this.Id = id;
            this.ParkOpenWaves = parkOpenWaves;
        }
    }
}

