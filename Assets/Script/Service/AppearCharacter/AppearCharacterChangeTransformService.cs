using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class AppearCharacterChangeTransformService
    {
        private readonly IPlayerAppearCharacterViewRepository playerAppearCharacterViewRepository;

        public AppearCharacterChangeTransformService(IPlayerAppearCharacterViewRepository playerAppearCharacterViewRepository)
        {
            this.playerAppearCharacterViewRepository = playerAppearCharacterViewRepository;            
        }

        public void Execute(PlayerAppearCharacterViewModel playerAppearCharacterViewModel, Vector3 position, Vector3 rotation) {
            playerAppearCharacterViewModel.UpdateTransform(position, rotation);
            playerAppearCharacterViewRepository.Store(playerAppearCharacterViewModel);            
        }
    }   
}
