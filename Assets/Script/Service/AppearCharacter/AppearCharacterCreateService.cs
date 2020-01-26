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
            AppearCharacterLifeDirectorType appearCharacterLifeDirectorType,
            PlayerAppearCharacterReserveModel playerAppearCharacterReserveModel,
            MovePath movePath
        ) {
            return playerAppearCharacterViewRepository.Create(
                appearCharacterModel,
                position,
                rotation,
                appearCharacterLifeDirectorType,
                playerAppearCharacterReserveModel,
                movePath
            );
        }
    }   
}
