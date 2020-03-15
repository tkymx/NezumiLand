using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class DailyAppearCharacterRegistReserveCreateService
    {
        private readonly IPlayerAppearCharacterReserveRepository playerAppearCharacterReserveRepository;

        public DailyAppearCharacterRegistReserveCreateService(IPlayerAppearCharacterReserveRepository playerAppearCharacterReserveRepository)
        {
            this.playerAppearCharacterReserveRepository = playerAppearCharacterReserveRepository;            
        }

        public PlayerAppearCharacterReserveModel Execute(            
            AppearCharacterLifeDirectorType appearCharacterLifeDirectorType,
            AppearCharacterDirectorModelBase appearCharacterDirectorModelBase,
            IDailyAppearCharacterRegistCondition dailyAppearCharacterRegistCondition
        ) {
            return playerAppearCharacterReserveRepository.Create(
                appearCharacterLifeDirectorType,
                appearCharacterDirectorModelBase,
                dailyAppearCharacterRegistCondition
            );
        }
    }   
}
