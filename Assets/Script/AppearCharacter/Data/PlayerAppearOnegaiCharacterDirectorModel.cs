using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class PlayerAppearOnegaiCharacterDirectorModel : PlayerAppearCharacterDirectorModelBase
    {
        public AppearOnegaiCharacterDirectorModel AppearOnegaiCharacterDirectorModel { get; private set; }
        public PlayerAppearCharacterReserveModel PlayerAppearCharacterReserveModel { get; private set; } 

        public PlayerAppearOnegaiCharacterDirectorModel(
            uint id,
            AppearOnegaiCharacterDirectorModel appearOnegaiCharacterDirectorModel,
            PlayerAppearCharacterReserveModel playerAppearCharacterReserveModel)
        {
            this.Id = id;
            this.AppearOnegaiCharacterDirectorModel = appearOnegaiCharacterDirectorModel;
            this.PlayerAppearCharacterReserveModel = playerAppearCharacterReserveModel;
        }
    }   
}