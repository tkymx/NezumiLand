using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class PlayerAppearConversationCharacterDirectorModel : PlayerAppearCharacterDirectorModelBase
    {
        public AppearConversationCharacterDirectorModel AppearConversationCharacterDirectorModel { get; private set; }
        public PlayerAppearCharacterReserveModel PlayerAppearCharacterReserveModel { get; private set; } 
        public bool IsReceiveReward { get; private set; }

        public PlayerAppearConversationCharacterDirectorModel(
            uint id,
            AppearConversationCharacterDirectorModel appearConversationCharacterDirectorModel,
            PlayerAppearCharacterReserveModel playerAppearCharacterReserveModel,
            bool isReceiveReward)
        {
            this.Id = id;
            this.AppearConversationCharacterDirectorModel = appearConversationCharacterDirectorModel;
            this.PlayerAppearCharacterReserveModel = playerAppearCharacterReserveModel;
            this.IsReceiveReward = isReceiveReward;
        }

        public void ToReceiveRewards() {
            this.IsReceiveReward = true;
        }
    }   
}