
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
        public PlayerParkOpenGroupModel PlayerParkOpenGroupModel { get; private set; }
        public int currentHeartCount { get; private set; }
        public PlayerParkOpenDeckModel CurrentParkOpenDeckModel { get; private set; }
        public bool CanUseCard1 { get; private set; }
        public bool CanUseCard2 { get; private set; }
        public bool CanUseCard3 { get; private set; }

        public PlayerParkOpenModel(
            uint id,
            bool isOpen,
            float elapsedTime,
            int nextWave,
            PlayerParkOpenGroupModel playerParkOpenGroupModel,
            int currentHeartCount,
            PlayerParkOpenDeckModel currentParkOpenDeckModel,
            bool canUseCard1,
            bool canUseCard2,
            bool canUseCard3
        )
        {
            this.Id = id;
            this.IsOpen = isOpen;
            this.ElapsedTime = elapsedTime;
            this.NextWave = nextWave;
            this.PlayerParkOpenGroupModel = playerParkOpenGroupModel;
            this.currentHeartCount = currentHeartCount;
            this.CurrentParkOpenDeckModel = currentParkOpenDeckModel;
            this.CanUseCard1 = canUseCard1;
            this.CanUseCard2 = canUseCard2;
            this.CanUseCard3 = canUseCard3;
        }

        public void Update(
            bool isOpen,
            float elapsedTime,
            int nextWave,
            PlayerParkOpenGroupModel playerParkOpenGroupModel,
            int currentHeartCount
        )
        {
            this.IsOpen = isOpen;
            this.ElapsedTime = elapsedTime;
            this.NextWave = nextWave;
            this.PlayerParkOpenGroupModel = playerParkOpenGroupModel;
            this.currentHeartCount = currentHeartCount;
        }

        public void AddHeartCount(int increaseCount = 1)
        {
            this.currentHeartCount += increaseCount;
        }

        public void SetParkOpenDeck(PlayerParkOpenDeckModel playerParkOpenDeckModel) {
            this.CurrentParkOpenDeckModel = playerParkOpenDeckModel;
        }

        public void ResetCardUse()
        {
            this.CanUseCard1 = false;
            this.CanUseCard2 = false;
            this.CanUseCard3 = false;
        }

        public void UseCard(PlayerParkOpenDeckModel.CountType countType)
        {
            switch (countType)
            {
                case PlayerParkOpenDeckModel.CountType.First:
                {
                    this.CanUseCard1 = true;
                    break;
                }
                case PlayerParkOpenDeckModel.CountType.Second:
                {
                    this.CanUseCard2 = true;
                    break;
                }
                case PlayerParkOpenDeckModel.CountType.Third:
                {
                    this.CanUseCard3 = true;
                    break;
                }
                default:
                {
                    break;
                }
            }
        }
    }
}

