using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class AppearCharacterReceiveRewardsService
    {
        private readonly IPlayerAppearCharacterViewRepository playerAppearCharacterViewRepository;

        public AppearCharacterReceiveRewardsService(IPlayerAppearCharacterViewRepository playerAppearCharacterViewRepository)
        {
            this.playerAppearCharacterViewRepository = playerAppearCharacterViewRepository;            
        }

        public void Execute(PlayerAppearCharacterViewModel playerAppearCharacterViewModel) {
            playerAppearCharacterViewModel.ToReceiveRewards();
            playerAppearCharacterViewRepository.Store(playerAppearCharacterViewModel);            
        }
    }   
}
