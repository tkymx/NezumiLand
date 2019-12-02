using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace NL.EventContents {
    public class AppearConversationCharacter : EventContentsBase {
        private readonly ConversationModel conversationModel = null;
        private readonly AppearCharacterModel appearCharacterModel = null;

        public AppearConversationCharacter(PlayerEventModel playerEventModel) : base(playerEventModel)
        {
            // TODO : この辺のレポジトリを使いまわしできるようにしたい。
            var conversationRepository = new ConversationRepository(ContextMap.DefaultMap);
            var appearCharacterRepository = new AppearCharacterRepository(ContextMap.DefaultMap);

            Debug.Assert(playerEventModel.EventModel.EventContentsModel.Arg.Length >= 2, "AppearConversationCharacter: コンテンツ引数の要素数が2未満です");
            var appearCharacterId = uint.Parse(playerEventModel.EventModel.EventContentsModel.Arg[0]);
            var conversationId = uint.Parse(playerEventModel.EventModel.EventContentsModel.Arg[1]);

            this.conversationModel = conversationRepository.Get(conversationId);
            this.appearCharacterModel = appearCharacterRepository.Get(appearCharacterId);
        }

        public override EventContentsType EventContentsType => EventContentsType.AppearConversationCharacter;

        public override void OnEnter() {
            GameManager.Instance.DailyAppearCharacterRegistManager.RegistReserve(new DailyAppearCharacterGeneratorResistReserve(
                new AppearCharacterGenerator(this.appearCharacterModel, this.conversationModel),
                new DailyAppearCharacterRegistConditionForce()
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