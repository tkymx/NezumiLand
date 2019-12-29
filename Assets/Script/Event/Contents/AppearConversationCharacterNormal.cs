using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace NL.EventContents {
    public class AppearConversationCharacterNormal : EventContentsBase {
        private readonly ConversationModel conversationModel = null;
        private readonly AppearCharacterModel appearCharacterModel = null;
        private float lot = 0;

        public AppearConversationCharacterNormal(PlayerEventModel playerEventModel) : base(playerEventModel)
        {
            // TODO : この辺のレポジトリを使いまわしできるようにしたい。
            var conversationRepository = new ConversationRepository(ContextMap.DefaultMap);
            var appearCharacterRepository = new AppearCharacterRepository(ContextMap.DefaultMap);
            var rewardRepository = new RewardRepository(ContextMap.DefaultMap);

            Debug.Assert(playerEventModel.EventModel.EventContentsModel.Arg.Length >= 3, "AppearConversationCharacter: コンテンツ引数の要素数が3未満です");
            var appearCharacterId = uint.Parse(playerEventModel.EventModel.EventContentsModel.Arg[0]);
            var conversationId = uint.Parse(playerEventModel.EventModel.EventContentsModel.Arg[1]);
            this.lot = float.Parse(playerEventModel.EventModel.EventContentsModel.Arg[2]);

            this.appearCharacterModel = appearCharacterRepository.Get(appearCharacterId);
            this.conversationModel = conversationRepository.Get(conversationId);
        }

        public override EventContentsType EventContentsType => EventContentsType.AppearConversationCharacterNormal;

        public override void OnEnter() {
            GameManager.Instance.DailyAppearCharacterRegistManager.RegistReserve(new DailyAppearCharacterGeneratorResistReserve(
                new AppearCharacterGenerator(this.appearCharacterModel, this.conversationModel, null),
                new DailyAppearCharacterRegistConditionByChance(this.lot)
            ));
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