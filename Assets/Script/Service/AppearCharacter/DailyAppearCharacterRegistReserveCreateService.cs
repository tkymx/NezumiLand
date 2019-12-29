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
            AppearCharacterModel appearCharacterModel,
            ConversationModel conversationModel,
            RewardModel rewardModel,
            IDailyAppearCharacterRegistCondition dailyAppearCharacterRegistCondition
        ) {
            return playerAppearCharacterReserveRepository.Create(
                appearCharacterModel,
                conversationModel,
                rewardModel,
                dailyAppearCharacterRegistCondition
            );
        }
    }   
}
