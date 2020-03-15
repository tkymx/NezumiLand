using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace NL.EventContents {

    /// <summary>
    /// AppearConversationCharacterNormal
    /// 会話するキャラクタを予約する
    /// 予約されたキャラクタは確率で登場することになる
    /// </summary>
    public class AppearConversationCharacterNormal : EventContentsBase {
        private readonly AppearConversationCharacterDirectorModel appearConversationCharacterDirectorModel = null;
        private readonly float rate = 0;

        public AppearConversationCharacterNormal(PlayerEventModel playerEventModel) : base(playerEventModel)
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

            Debug.Assert(playerEventModel.EventModel.EventContentsModel.Arg.Length >= 2, "AppearConversationCharacterNormal: コンテンツ引数の要素数が 2 未満です");
            var appearConversationCharacterDirectorId = uint.Parse(playerEventModel.EventModel.EventContentsModel.Arg[0]);
            this.appearConversationCharacterDirectorModel = appearConversationCharacterDirectorRepository.Get(appearConversationCharacterDirectorId);
            this.rate = float.Parse(playerEventModel.EventModel.EventContentsModel.Arg[1]);
        }

        public override EventContentsType EventContentsType => EventContentsType.AppearConversationCharacterNormal;

        public override void OnEnter() {
            GameManager.Instance.DailyAppearCharacterRegistManager.RegistReserve(
                AppearCharacterLifeDirectorType.Conversation,
                this.appearConversationCharacterDirectorModel,                
                new DailyAppearCharacterRegistConditionByChance(this.rate));
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