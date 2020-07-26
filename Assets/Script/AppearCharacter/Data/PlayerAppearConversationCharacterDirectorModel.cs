using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class AppearCharactorWithReward 
    {
        public bool IsReceiveReward { get; private set; }

        public AppearCharactorWithReward(bool isReceiveReward)
        {
            this.IsReceiveReward = isReceiveReward;
        }    

        public void ToReceiveRewards() {
            this.IsReceiveReward = true;
        }
    }

    public class PlayerAppearConversationCharacterDirectorModel : PlayerAppearCharacterDirectorModelBase
    {
        public AppearConversationCharacterDirectorModel AppearConversationCharacterDirectorModel { get; private set; }
        public PlayerAppearCharacterReserveModel PlayerAppearCharacterReserveModel { get; private set; } 
        public AppearCharactorWithReward AppearCharactorWithReward { get; private set; }

        public PlayerAppearConversationCharacterDirectorModel(
            uint id,
            AppearConversationCharacterDirectorModel appearConversationCharacterDirectorModel,
            PlayerAppearCharacterReserveModel playerAppearCharacterReserveModel,
            AppearCharactorWithReward appearCharactorWithReward)
        {
            this.Id = id;
            this.AppearConversationCharacterDirectorModel = appearConversationCharacterDirectorModel;
            this.PlayerAppearCharacterReserveModel = playerAppearCharacterReserveModel;
            this.AppearCharactorWithReward = appearCharactorWithReward;
        }

        public override IAppearCharacterLifeDirector GetLifeDirectorFromViewModel(AppearCharacterViewModel appearCharacterViewModel)
        {
            return new ReserveAppearCharacterLifeDirector(this);
        }
    }   
}