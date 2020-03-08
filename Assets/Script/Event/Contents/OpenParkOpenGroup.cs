using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace NL.EventContents {
    public class OpenParkOpenGroup : EventContentsBase {
        private readonly ParkOpenGroupModel parkOpenGroupModel = null;

        public OpenParkOpenGroup(PlayerEventModel playerEventModel) : base(playerEventModel)
        {
            // TODO : この辺のレポジトリを使いまわしできるようにしたい。
            var appearCharacterRepository = new AppearCharacterRepository(ContextMap.DefaultMap);
            var parkOpenWaveRepository = new ParkOpenWaveRepository(appearCharacterRepository, ContextMap.DefaultMap);
            var rewardRepository = new RewardRepository(ContextMap.DefaultMap);
            var parkOpenGroupRepository = new ParkOpenGroupRepository(parkOpenWaveRepository, rewardRepository, ContextMap.DefaultMap);

            Debug.Assert(playerEventModel.EventModel.EventContentsModel.Arg.Length >= 1, "OpenParkOpenGroup: コンテンツ引数の要素数が1未満です");
            var openParkGroupId = uint.Parse(playerEventModel.EventModel.EventContentsModel.Arg[0]);

            this.parkOpenGroupModel = parkOpenGroupRepository.Get(openParkGroupId);
        }

        public override EventContentsType EventContentsType => EventContentsType.AppearConversationCharacter;

        public override void OnEnter() {
            GameManager.Instance.ParkOpenGroupManager.ToOpen(this.parkOpenGroupModel);
        }
        public override void OnUpdate() {
        }
        public override void OnExit() {
        }
        public override bool IsAlive() {
            return false;
        }

        public override string ToString() {
            return this.EventContentsType.ToString() + " " + this.parkOpenGroupModel.Id.ToString();
        }
    }
}