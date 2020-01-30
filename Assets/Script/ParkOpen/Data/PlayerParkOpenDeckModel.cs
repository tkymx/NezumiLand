using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class PlayerParkOpenDeckModel : ModelBase
    {
        public PlayerParkOpenCardModel PlayerParkOpenCardModel1 { get; private set; }
        public PlayerParkOpenCardModel PlayerParkOpenCardModel2 { get; private set; }
        public PlayerParkOpenCardModel PlayerParkOpenCardModel3 { get; private set; }

        public PlayerParkOpenDeckModel(
            uint id,
            PlayerParkOpenCardModel playerParkOpenCardModel1,
            PlayerParkOpenCardModel playerParkOpenCardModel2,
            PlayerParkOpenCardModel playerParkOpenCardModel3)
        {
            this.Id = id;
            this.PlayerParkOpenCardModel1 = playerParkOpenCardModel1;
            this.PlayerParkOpenCardModel2 = playerParkOpenCardModel2;
            this.PlayerParkOpenCardModel3 = playerParkOpenCardModel3;
        }
    }   
}