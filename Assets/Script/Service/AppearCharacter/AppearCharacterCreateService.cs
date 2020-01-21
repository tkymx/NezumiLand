using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class AppearCharacterCreateService
    {
        private readonly IPlayerAppearCharacterViewRepository playerAppearCharacterViewRepository;

        public AppearCharacterCreateService(IPlayerAppearCharacterViewRepository playerAppearCharacterViewRepository)
        {
            this.playerAppearCharacterViewRepository = playerAppearCharacterViewRepository;            
        }

        public PlayerAppearCharacterViewModel Execute(
            AppearCharacterModel appearCharacterModel,
            Vector3 position,
            Vector3 rotation,
            PlayerAppearCharacterReserveModel playerAppearCharacterReserveModel,
            AppearCharacterLifeDirectorType appearCharacterLifeDirectorType
        ) {
            return playerAppearCharacterViewRepository.Create(
                appearCharacterModel,
                position,
                rotation,
                playerAppearCharacterReserveModel,
                appearCharacterLifeDirectorType
            );
        }
    }   
}
