using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class PlayerAppearPlayingCharacterDirectorModel : PlayerAppearCharacterDirectorModelBase
    {
        public AppearPlayingCharacterDirectorModel AppearPlayingCharacterDirectorModel { get; private set; }
        public PlayerAppearCharacterReserveModel PlayerAppearCharacterReserveModel { get; private set; } 

        public PlayerAppearPlayingCharacterDirectorModel(
            uint id,
            AppearPlayingCharacterDirectorModel appearPlayingCharacterDirectorModel,
            PlayerAppearCharacterReserveModel playerAppearCharacterReserveModel)
        {
            this.Id = id;
            this.AppearPlayingCharacterDirectorModel = appearPlayingCharacterDirectorModel;
            this.PlayerAppearCharacterReserveModel = playerAppearCharacterReserveModel;            
        }

        public override IAppearCharacterLifeDirector GetLifeDirectorFromViewModel(AppearCharacterViewModel appearCharacterViewModel)
        {
            return new AppearPlayingCharacterLifeDirector(appearCharacterViewModel, this);
        }        
    }   
}