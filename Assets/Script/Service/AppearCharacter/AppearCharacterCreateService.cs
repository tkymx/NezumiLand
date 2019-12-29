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
            Vector3 position,
            Vector3 rotation,
            PlayerAppearCharacterReserveModel playerAppearCharacterReserveModel
        ) {
            return playerAppearCharacterViewRepository.Create(
                position,
                rotation,
                playerAppearCharacterReserveModel
            );
        }
    }   
}
