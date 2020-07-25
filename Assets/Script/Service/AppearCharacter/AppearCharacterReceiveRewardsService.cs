using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class AppearCharacterReceiveRewardsService
    {
        private readonly IPlayerAppearConversationCharacterDirectorRepository playerAppearConversationCharacterDirectorRepository;
        private readonly IPlayerAppearOnegaiCharacterDirectorRepository playerAppearOnegaiCharacterDirectorRepository;

        public AppearCharacterReceiveRewardsService(
            IPlayerAppearConversationCharacterDirectorRepository playerAppearConversationCharacterDirectorRepository,
            IPlayerAppearOnegaiCharacterDirectorRepository playerAppearOnegaiCharacterDirectorRepository)
        {
            this.playerAppearConversationCharacterDirectorRepository = playerAppearConversationCharacterDirectorRepository;   
            this.playerAppearOnegaiCharacterDirectorRepository = playerAppearOnegaiCharacterDirectorRepository;         
        }

        // conversation
        public void Execute(PlayerAppearConversationCharacterDirectorModel playerAppearConversationCharacterDirectorModel) {
            playerAppearConversationCharacterDirectorModel.AppearCharactorWithReward.ToReceiveRewards();
            playerAppearConversationCharacterDirectorRepository.Store(playerAppearConversationCharacterDirectorModel);            
        }
    }   
}
