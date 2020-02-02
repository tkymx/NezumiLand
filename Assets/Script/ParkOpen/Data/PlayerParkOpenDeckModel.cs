using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class PlayerParkOpenDeckModel : ModelBase
    {
        public enum CountType
        {
            First,
            Second,
            Third
        };

        private Dictionary<CountType, PlayerParkOpenCardModel> playerParkOpenCardModelDics;

        public PlayerParkOpenCardModel FirstCardModel => this.GetDeckCardModel(CountType.First);
        public PlayerParkOpenCardModel SecondCardModel => this.GetDeckCardModel(CountType.Second);
        public PlayerParkOpenCardModel ThirdCardModel => this.GetDeckCardModel(CountType.Third);

        private PlayerParkOpenCardModel GetDeckCardModel(CountType countType)
        {
            if (!this.playerParkOpenCardModelDics.ContainsKey(countType))
            {
                return null;
            }
            return this.playerParkOpenCardModelDics[countType];
        }

        public PlayerParkOpenDeckModel(
            uint id,
            PlayerParkOpenCardModel playerParkOpenCardModel1,
            PlayerParkOpenCardModel playerParkOpenCardModel2,
            PlayerParkOpenCardModel playerParkOpenCardModel3)
        {
            this.playerParkOpenCardModelDics = new Dictionary<CountType, PlayerParkOpenCardModel>();

            this.Id = id;
            this.playerParkOpenCardModelDics[CountType.First] = playerParkOpenCardModel1;
            this.playerParkOpenCardModelDics[CountType.Second] = playerParkOpenCardModel2;
            this.playerParkOpenCardModelDics[CountType.Third] = playerParkOpenCardModel3;
        }

        public void SetCard(CountType countType, PlayerParkOpenCardModel playerParkOpenCardModel)
        {
            this.playerParkOpenCardModelDics[countType] = playerParkOpenCardModel;
        }
    }   
}