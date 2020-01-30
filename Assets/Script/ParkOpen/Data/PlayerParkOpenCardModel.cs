using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class PlayerParkOpenCardModel : ModelBase
    {
        public ParkOpenCardModel ParkOpenCardModel { get; private set; }

        public PlayerParkOpenCardModel(
            uint id,
            ParkOpenCardModel parkOpenCardModel)
        {
            this.Id = id;
            this.ParkOpenCardModel = parkOpenCardModel;
        }
    }   
}