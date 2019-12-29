using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class PlayerAppearCharacterReserveModel : ModelBase
    {
        public AppearCharacterModel AppearCharacterModel { get; private set; }
        public  ConversationModel ConversationModel { get; private set; }
        public RewardModel RewardModel { get; private set; }
        public IDailyAppearCharacterRegistCondition DailyAppearCharacterRegistCondition { get; private set; }
        public bool IsNextRemove { get; private set; }        

        public PlayerAppearCharacterReserveModel(
            uint id,
            AppearCharacterModel appearCharacterModel,
            ConversationModel conversationModel,
            RewardModel rewardModel,
            IDailyAppearCharacterRegistCondition dailyAppearCharacterRegistCondition,
            bool isNextRemove)
        {
            this.Id = id;
            this.AppearCharacterModel = appearCharacterModel;
            this.ConversationModel = conversationModel;
            this.RewardModel = rewardModel;
            this.DailyAppearCharacterRegistCondition = dailyAppearCharacterRegistCondition;
            this.IsNextRemove = isNextRemove;
        }

        public void NextRemove() {
            this.IsNextRemove = true;
        }
    }   
}