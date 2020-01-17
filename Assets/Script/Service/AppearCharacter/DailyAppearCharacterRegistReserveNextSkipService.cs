using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class DailyAppearCharacterRegistReserveNextSkipService
    {
        private readonly IPlayerAppearCharacterReserveRepository playerAppearCharacterReserveRepository;

        public DailyAppearCharacterRegistReserveNextSkipService(IPlayerAppearCharacterReserveRepository playerAppearCharacterReserveRepository)
        {
            this.playerAppearCharacterReserveRepository = playerAppearCharacterReserveRepository;            
        }

        public void Execute(PlayerAppearCharacterReserveModel playerAppearCharacterReserveModel, bool isNextSkip) {
            playerAppearCharacterReserveModel.SetNextSkippable(isNextSkip);
            playerAppearCharacterReserveRepository.Store(playerAppearCharacterReserveModel);
        }
    }   
}
