using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    /// <summary>
    /// 遊園地公開時のキャラクタ出現のウェーブ
    /// </summary>
    public class ParkOpenWaveModel : ModelBase
    {
        public int AppearCount { get; private set; }
        public int FluctuationCount { get; private set; }
        public AppearParkOpenCharacterDirectorModel[] AppearParkOpenCharacterDirectorModels; 

        public ParkOpenWaveModel(
            uint id,
            int appearCount,
            int fluctuationCount,
            AppearParkOpenCharacterDirectorModel[] appearParkOpenCharacterDirectorModels
        )
        {
            this.Id = id;
            this.AppearCount = appearCount;
            this.FluctuationCount = fluctuationCount;
            this.AppearParkOpenCharacterDirectorModels = appearParkOpenCharacterDirectorModels;
        }
    }
}

