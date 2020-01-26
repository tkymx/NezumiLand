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
        public AppearCharacterModel[] AppearCharacterModels; 

        public ParkOpenWaveModel(
            uint id,
            int appearCount,
            int fluctuationCount,
            AppearCharacterModel[] appearCharacterModels
        )
        {
            this.Id = id;
            this.AppearCount = appearCount;
            this.FluctuationCount = fluctuationCount;
            this.AppearCharacterModels = appearCharacterModels;
        }
    }
}

