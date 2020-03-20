using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    /// <summary>
    /// 会話するキャラクタの動き情報
    /// </summary>
    public class AppearOnegaiCharacterDirectorModel : AppearCharacterDirectorModelBase
    {
        public ConversationModel AfterConversationModel { get; private set; }
        public ConversationModel BeforeConversationModel { get; private set; }
        public ConversationModel CancelConversationModel { get; private set; }
        public OnegaiModel OnegaiModel { get; private set; }
        public RewardModel RewardModel { get; private set; }

        public AppearOnegaiCharacterDirectorModel(
            uint id,
            AppearCharacterModel appearCharacterModel,
            ConversationModel afterConversationModel,
            ConversationModel beforeConversationModel,
            ConversationModel cancelConversationModel,
            OnegaiModel onegaiModel,
            RewardModel rewardModel
        )
        {
            this.Id = id;
            this.AppearCharacterModel = appearCharacterModel;
            this.AfterConversationModel = afterConversationModel;
            this.BeforeConversationModel = beforeConversationModel;
            this.CancelConversationModel = cancelConversationModel;
            this.OnegaiModel = onegaiModel;
            this.RewardModel = rewardModel;
        }
    }
}

