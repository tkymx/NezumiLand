using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class AppearCharacterSetTargetArrangementService
    {
        private readonly IPlayerAppearCharacterViewRepository playerAppearCharacterViewRepository;

        public AppearCharacterSetTargetArrangementService(IPlayerAppearCharacterViewRepository playerAppearCharacterViewRepository)
        {
            this.playerAppearCharacterViewRepository = playerAppearCharacterViewRepository;            
        }

        public void Execute(PlayerAppearCharacterViewModel playerAppearCharacterViewModel, PlayerArrangementTargetModel playerArrangementTargetModel) {
            playerAppearCharacterViewModel.SetTargetArrangement(playerArrangementTargetModel);
            playerAppearCharacterViewRepository.Store(playerAppearCharacterViewModel);            
        }
    }   
}
