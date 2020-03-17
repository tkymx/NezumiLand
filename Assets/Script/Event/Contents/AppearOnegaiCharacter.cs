using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace NL.EventContents {

    /// <summary>
    /// AppearOnegaiCharacter
    /// お願いするキャラクタを予約する
    /// お願いクリア前は、タップで会話後にお願いを表示する。
    /// お願いクリア後に、タップで会話後に報酬をくれる
    /// 予約されたキャラクタは必ず登場して会話後にいなくなる
    /// </summary>
    public class AppearOnegaiCharacter : EventContentsBase {
        private readonly AppearOnegaiCharacterDirectorModel appearOnegaiCharacterDirectorModel;

        public AppearOnegaiCharacter(PlayerEventModel playerEventModel) : base(playerEventModel)
        {
            // TODO : この辺のレポジトリを使いまわしできるようにしたい。
            var conversationRepository = new ConversationRepository(ContextMap.DefaultMap);
            var appearCharacterRepository = new AppearCharacterRepository(ContextMap.DefaultMap);
            var scheduleRepository = new ScheduleRepository(ContextMap.DefaultMap);
            var onegaiRepository = new OnegaiRepository(ContextMap.DefaultMap, scheduleRepository);
            var rewardRepository = new RewardRepository(ContextMap.DefaultMap);
            var appearOnegaiCharacterDirectorRepository = new AppearOnegaiCharacterDirectorRepository(appearCharacterRepository, conversationRepository, onegaiRepository, rewardRepository, ContextMap.DefaultMap);

            Debug.Assert(playerEventModel.EventModel.EventContentsModel.Arg.Length >= 1, "AppearOnegaiCharacter: コンテンツ引数の要素数が1未満です");
            var appearOnegaiCharacterDirectorId = uint.Parse(playerEventModel.EventModel.EventContentsModel.Arg[0]);
            this.appearOnegaiCharacterDirectorModel = appearOnegaiCharacterDirectorRepository.Get(appearOnegaiCharacterDirectorId);
        }

        public override EventContentsType EventContentsType => EventContentsType.AppearConversationCharacter;

        public override void OnEnter() {
            GameManager.Instance.DailyAppearCharacterRegistManager.RegistReserve(
                AppearCharacterLifeDirectorType.Onegai,
                this.appearOnegaiCharacterDirectorModel,
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
            return this.EventContentsType.ToString() + " " + appearOnegaiCharacterDirectorModel.Id.ToString();
        }
    }
}