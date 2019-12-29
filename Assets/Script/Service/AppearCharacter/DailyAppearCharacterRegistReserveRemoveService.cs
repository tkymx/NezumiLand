using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class DailyAppearCharacterRegistReserveRemoveService
    {
        private readonly IPlayerAppearCharacterReserveRepository playerAppearCharacterReserveRepository;

        public DailyAppearCharacterRegistReserveRemoveService(IPlayerAppearCharacterReserveRepository playerAppearCharacterReserveRepository)
        {
            this.playerAppearCharacterReserveRepository = playerAppearCharacterReserveRepository;            
        }

        public void Execute(PlayerAppearCharacterReserveModel playerAppearCharacterReserveModel) {
            playerAppearCharacterReserveRepository.Remove(playerAppearCharacterReserveModel);
        }
    }   
}
