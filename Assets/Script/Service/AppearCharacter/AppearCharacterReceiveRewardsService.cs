using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class AppearCharacterReceiveRewardsService
    {
        private readonly IPlayerAppearConversationCharacterDirectorRepository playerAppearConversationCharacterDirectorRepository;

        public AppearCharacterReceiveRewardsService(IPlayerAppearConversationCharacterDirectorRepository playerAppearConversationCharacterDirectorRepository)
        {
            this.playerAppearConversationCharacterDirectorRepository = playerAppearConversationCharacterDirectorRepository;            
        }

        public void Execute(PlayerAppearConversationCharacterDirectorModel playerAppearConversationCharacterDirectorModel) {
            playerAppearConversationCharacterDirectorModel.ToReceiveRewards();
            playerAppearConversationCharacterDirectorRepository.Store(playerAppearConversationCharacterDirectorModel);            
        }
    }   
}
