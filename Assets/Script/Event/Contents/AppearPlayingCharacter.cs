using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace NL.EventContents {

    /// <summary>
    /// AppearPlayingCharacter
    /// 遊具で遊ぶキャラクタを定義する
    /// </summary>
    public class AppearPlayingCharacter : EventContentsBase {
        private readonly AppearPlayingCharacterDirectorModel appearPlayingCharacterDirectorModel;
        private float rate;

        public AppearPlayingCharacter(PlayerEventModel playerEventModel) : base(playerEventModel)
        {
            var appearCharacterRepository = new AppearCharacterRepository(ContextMap.DefaultMap);
            var appearPlayingCharacterDirectorRepository = new AppearPlayingCharacterDirectorRepository(appearCharacterRepository, ContextMap.DefaultMap);

            Debug.Assert(playerEventModel.EventModel.EventContentsModel.Arg.Length >= 2, "AppearPlayingCharacter: コンテンツ引数の要素数が1未満です");

            // DirectorのID
            var appearPlayingCharacterDirectorId = uint.Parse(playerEventModel.EventModel.EventContentsModel.Arg[0]);
            this.appearPlayingCharacterDirectorModel = appearPlayingCharacterDirectorRepository.Get(appearPlayingCharacterDirectorId);

            // 出現確率
            this.rate = float.Parse(playerEventModel.EventModel.EventContentsModel.Arg[1]);
        }

        public override EventContentsType EventContentsType => EventContentsType.AppearConversationCharacter;

        public override void OnEnter() {
            GameManager.Instance.DailyAppearCharacterRegistManager.RegistReserve(
                AppearCharacterLifeDirectorType.Playing,
                this.appearPlayingCharacterDirectorModel,
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
            return this.EventContentsType.ToString() + " " + appearPlayingCharacterDirectorModel.Id.ToString();
        }
    }
}