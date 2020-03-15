using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace NL.EventContents {

    /// <summary>
    /// AppearConversationCharacter
    /// お願いするキャラクタを予約する
    /// お願いクリア前は、タップで会話後にお願いを表示する。
    /// お願いクリア後に、タップで会話後に報酬をくれる
    /// 予約されたキャラクタは必ず登場して会話後にいなくなる
    /// </summary>
    public class AppearOnegaiCharacterForce : EventContentsBase {
        private readonly ConversationModel afterConversationModel = null;
        private readonly ConversationModel beforeConversationModel = null;
        private readonly OnegaiModel onegaiModel = null;
        private readonly AppearCharacterModel appearCharacterModel = null;
        private readonly RewardModel rewardModel = null;

        public AppearOnegaiCharacterForce(PlayerEventModel playerEventModel) : base(playerEventModel)
        {
            // TODO : この辺のレポジトリを使いまわしできるようにしたい。
            var conversationRepository = new ConversationRepository(ContextMap.DefaultMap);
            var appearCharacterRepository = new AppearCharacterRepository(ContextMap.DefaultMap);
            var scheduleRepository = new ScheduleRepository(ContextMap.DefaultMap);
            var onegaiRepository = new OnegaiRepository(ContextMap.DefaultMap, scheduleRepository);
            var rewardRepository = new RewardRepository(ContextMap.DefaultMap);

            Debug.Assert(playerEventModel.EventModel.EventContentsModel.Arg.Length >= 5, "AppearOnegaiCharacterForce: コンテンツ引数の要素数が5未満です");
            var appearCharacterId = uint.Parse(playerEventModel.EventModel.EventContentsModel.Arg[0]);
            var afterConversationId = uint.Parse(playerEventModel.EventModel.EventContentsModel.Arg[1]);
            var beforeConversationId = uint.Parse(playerEventModel.EventModel.EventContentsModel.Arg[2]);
            var onegaiId = uint.Parse(playerEventModel.EventModel.EventContentsModel.Arg[3]);
            var rewardId = uint.Parse(playerEventModel.EventModel.EventContentsModel.Arg[4]);

            this.appearCharacterModel = appearCharacterRepository.Get(appearCharacterId);
            this.afterConversationModel = conversationRepository.Get(afterConversationId);
            this.beforeConversationModel = conversationRepository.Get(beforeConversationId);
            this.onegaiModel = onegaiRepository.Get(onegaiId);
            this.rewardModel = rewardRepository.Get(rewardId);
        }

        public override EventContentsType EventContentsType => EventContentsType.AppearConversationCharacter;

        public override void OnEnter() {
/*            GameManager.Instance.DailyAppearCharacterRegistManager.RegistReserve(
                this.appearCharacterModel, this.conversationModel, this.rewardModel,
                new DailyAppearCharacterRegistConditionForce());*/
        }
        public override void OnUpdate() {
        }
        public override void OnExit() {
        }
        public override bool IsAlive() {
            return false;
        }

        public override string ToString() {
            return this.EventContentsType.ToString() + " " + appearCharacterModel.Id.ToString() + " " + afterConversationModel.Id.ToString() + " " + beforeConversationModel.Id.ToString() + " " + onegaiModel.Id.ToString() + " " + rewardModel.Id.ToString();
        }
    }
}