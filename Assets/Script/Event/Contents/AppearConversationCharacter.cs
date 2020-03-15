using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace NL.EventContents {

    /// <summary>
    /// AppearConversationCharacter
    /// 会話するキャラクタを予約する
    /// 予約されたキャラクタは必ず登場して会話後にいなくなる
    /// </summary>
    public class AppearConversationCharacter : EventContentsBase {
        private readonly AppearConversationCharacterDirectorModel appearConversationCharacterDirectorModel = null;

        public AppearConversationCharacter(PlayerEventModel playerEventModel) : base(playerEventModel)
        {
            // TODO : この辺のレポジトリを使いまわしできるようにしたい。
            var conversationRepository = new ConversationRepository(ContextMap.DefaultMap);
            var appearCharacterRepository = new AppearCharacterRepository(ContextMap.DefaultMap);
            var rewardRepository = new RewardRepository(ContextMap.DefaultMap);
            var appearConversationCharacterDirectorRepository = new AppearConversationCharacterDirectorRepository(
                appearCharacterRepository,
                conversationRepository,
                rewardRepository,
                ContextMap.DefaultMap);

            Debug.Assert(playerEventModel.EventModel.EventContentsModel.Arg.Length >= 1, "AppearConversationCharacter: コンテンツ引数の要素数が1未満です");
            var appearConversationCharacterDirectorId = uint.Parse(playerEventModel.EventModel.EventContentsModel.Arg[0]);
            this.appearConversationCharacterDirectorModel = appearConversationCharacterDirectorRepository.Get(appearConversationCharacterDirectorId);
        }

        public override EventContentsType EventContentsType => EventContentsType.AppearConversationCharacter;

        public override void OnEnter() {
            GameManager.Instance.DailyAppearCharacterRegistManager.RegistReserve(
                AppearCharacterLifeDirectorType.Conversation,
                this.appearConversationCharacterDirectorModel,
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
            return this.EventContentsType.ToString() + " " + appearConversationCharacterDirectorModel.Id.ToString();
        }
    }
}