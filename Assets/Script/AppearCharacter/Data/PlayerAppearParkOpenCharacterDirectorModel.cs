using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class PlayerAppearParkOpenCharacterDirectorModel : PlayerAppearCharacterDirectorModelBase
    {
        public AppearParkOpenCharacterDirectorModel appearParkOpenCharacterDirectorModel { get; private set; }

        public PlayerAppearParkOpenCharacterDirectorModel(
            uint id,
            AppearParkOpenCharacterDirectorModel appearParkOpenCharacterDirectorModel)
        {
            this.Id = id;
            this.appearParkOpenCharacterDirectorModel = appearParkOpenCharacterDirectorModel;
        }
    }   
}