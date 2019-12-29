using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace NL.EventContents {
    public class AppearConversationCharacter : EventContentsBase {
        private readonly ConversationModel conversationModel = null;
        private readonly AppearCharacterModel appearCharacterModel = null;
        private readonly RewardModel rewardModel = null;

        public AppearConversationCharacter(PlayerEventModel playerEventModel) : base(playerEventModel)
        {
            // TODO : この辺のレポジトリを使いまわしできるようにしたい。
            var conversationRepository = new ConversationRepository(ContextMap.DefaultMap);
            var appearCharacterRepository = new AppearCharacterRepository(ContextMap.DefaultMap);
            var rewardRepository = new RewardRepository(ContextMap.DefaultMap);

            Debug.Assert(playerEventModel.EventModel.EventContentsModel.Arg.Length >= 3, "AppearConversationCharacter: コンテンツ引数の要素数が3未満です");
            var appearCharacterId = uint.Parse(playerEventModel.EventModel.EventContentsModel.Arg[0]);
            var conversationId = uint.Parse(playerEventModel.EventModel.EventContentsModel.Arg[1]);
            var rewardId = uint.Parse(playerEventModel.EventModel.EventContentsModel.Arg[2]);

            this.appearCharacterModel = appearCharacterRepository.Get(appearCharacterId);
            this.conversationModel = conversationRepository.Get(conversationId);
            this.rewardModel = rewardRepository.Get(rewardId);
        }

        public override EventContentsType EventContentsType => EventContentsType.AppearConversationCharacter;

        public override void OnEnter() {
            GameManager.Instance.DailyAppearCharacterRegistManager.RegistReserve(
                this.appearCharacterModel, this.conversationModel, this.rewardModel,
                new DailyAppearCharacterRegistConditionForce());
        }
        public override void OnUpdate() {
        }
        public override void OnExit() {
        }
        public override bool IsAlive() {
            return false;
        }

        public override string ToString() {
            return this.EventContentsType.ToString() + " " + appearCharacterModel.Id.ToString() + " " + conversationModel.Id.ToString();
        }
    }
}