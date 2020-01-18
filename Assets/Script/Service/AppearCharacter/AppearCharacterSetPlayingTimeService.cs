using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class AppearCharacterSetPlayingTimeService
    {
        private readonly IPlayerAppearCharacterViewRepository playerAppearCharacterViewRepository;

        public AppearCharacterSetPlayingTimeService(IPlayerAppearCharacterViewRepository playerAppearCharacterViewRepository)
        {
            this.playerAppearCharacterViewRepository = playerAppearCharacterViewRepository;            
        }

        public void Execute(PlayerAppearCharacterViewModel playerAppearCharacterViewModel, float currentPlayingTime) {
            playerAppearCharacterViewModel.SetCurrentPlayingTime(currentPlayingTime);
            playerAppearCharacterViewRepository.Store(playerAppearCharacterViewModel);            
        }
    }   
}