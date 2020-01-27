
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    /// <summary>
    /// 遊園地公開時の横断的なプレイヤー情報
    /// </summary>
    public class PlayerParkOpenModel : ModelBase
    {
        public bool IsOpen { get; private set; }
        public float ElapsedTime { get; private set; }
        public int NextWave { get; private set; }
        public ParkOpenGroupModel ParkOpenGroupModel { get; private set; }
        public int currentHeartCount { get; private set; }

        public PlayerParkOpenModel(
            uint id,
            bool isOpen,
            float elapsedTime,
            int nextWave,
            ParkOpenGroupModel parkOpenGroupModel,
            int currentHeartCount
        )
        {
            this.Id = id;
            this.IsOpen = isOpen;
            this.ElapsedTime = elapsedTime;
            this.NextWave = nextWave;
            this.ParkOpenGroupModel = parkOpenGroupModel;
            this.currentHeartCount = currentHeartCount;
        }

        public void Update(
            bool isOpen,
            float elapsedTime,
            int nextWave,
            ParkOpenGroupModel parkOpenGroupModel,
            int currentHeartCount
        )
        {
            this.IsOpen = isOpen;
            this.ElapsedTime = elapsedTime;
            this.NextWave = nextWave;
            this.ParkOpenGroupModel = parkOpenGroupModel;
            this.currentHeartCount = currentHeartCount;
        }

        public void AddHeartCount(int increaseCount = 1)
        {
            this.currentHeartCount += increaseCount;
        }
    }
}

