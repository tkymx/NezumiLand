using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    /// <summary>
    /// 会話するキャラクタの動き情報
    /// </summary>
    public class AppearConversationCharacterDirectorModel : AppearCharacterDirectorModelBase
    {
        public ConversationModel ConversationModel { get; private set; }
        public RewardModel RewardModel { get; private set; }

        public AppearConversationCharacterDirectorModel(
            uint id,
            AppearCharacterModel appearCharacterModel,
            ConversationModel conversationModel,
            RewardModel rewardModel
        )
        {
            this.Id = id;
            this.AppearCharacterModel = appearCharacterModel;
            this.ConversationModel = conversationModel;
            this.RewardModel = rewardModel; 
        }
    }
}

