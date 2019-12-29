using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class AppearCharacterRemoveService
    {
        private readonly IPlayerAppearCharacterViewRepository playerAppearCharacterViewRepository;

        public AppearCharacterRemoveService(IPlayerAppearCharacterViewRepository playerAppearCharacterViewRepository)
        {
            this.playerAppearCharacterViewRepository = playerAppearCharacterViewRepository;            
        }

        public void Execute(PlayerAppearCharacterViewModel playerAppearCharacterViewModel) {
            playerAppearCharacterViewRepository.Remove(playerAppearCharacterViewModel);
        }
    }   
}
