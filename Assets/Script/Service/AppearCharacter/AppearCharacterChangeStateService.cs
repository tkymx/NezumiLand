using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class AppearCharacterChangeStateService
    {
        private readonly IPlayerAppearCharacterViewRepository playerAppearCharacterViewRepository;

        public AppearCharacterChangeStateService(IPlayerAppearCharacterViewRepository playerAppearCharacterViewRepository)
        {
            this.playerAppearCharacterViewRepository = playerAppearCharacterViewRepository;            
        }

        public void Execute(PlayerAppearCharacterViewModel playerAppearCharacterViewModel, AppearCharacterState appearCharacterState) {
            playerAppearCharacterViewModel.ChangeState(appearCharacterState);
            playerAppearCharacterViewRepository.Store(playerAppearCharacterViewModel);            
        }
    }   
}
