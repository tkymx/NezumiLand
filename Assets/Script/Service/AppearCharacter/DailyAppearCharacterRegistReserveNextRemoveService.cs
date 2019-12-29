using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class DailyAppearCharacterRegistReserveNextRemoveService
    {
        private readonly IPlayerAppearCharacterReserveRepository playerAppearCharacterReserveRepository;

        public DailyAppearCharacterRegistReserveNextRemoveService(IPlayerAppearCharacterReserveRepository playerAppearCharacterReserveRepository)
        {
            this.playerAppearCharacterReserveRepository = playerAppearCharacterReserveRepository;            
        }

        public void Execute(PlayerAppearCharacterReserveModel playerAppearCharacterReserveModel) {
            playerAppearCharacterReserveModel.NextRemove();
            playerAppearCharacterReserveRepository.Store(playerAppearCharacterReserveModel);
        }
    }   
}
