using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {

    /// <summary>
    /// 遊園地に登場するキャラクタの登場予約情報
    /// </summary>
    public class DailyAppearCharacterGeneratorResistReserve
    {
        public PlayerAppearCharacterReserveModel PlayerAppearCharacterReserveModel { get; private set; }

        public DailyAppearCharacterGeneratorResistReserve(PlayerAppearCharacterReserveModel playerAppearCharacterReserveModel)
        {
            this.PlayerAppearCharacterReserveModel = playerAppearCharacterReserveModel;
        }

        public bool IsResist() {
            return this.PlayerAppearCharacterReserveModel.DailyAppearCharacterRegistCondition.IsResist();
        }

        public bool IsOnce() {
            return this.PlayerAppearCharacterReserveModel.DailyAppearCharacterRegistCondition.IsOnce ();
        }

        public void Generate() {
            var generator = new AppearCharacterGenerator();
            GameManager.Instance.AppearCharacterManager.EnqueueRegister(generator.GenerateFromReserve(PlayerAppearCharacterReserveModel));
        }

        public override string ToString() {
            return this.PlayerAppearCharacterReserveModel.AppearCharacterDirectorModelBase.Id.ToString() + " " + this.PlayerAppearCharacterReserveModel.DailyAppearCharacterRegistCondition.ToString();  
        }
    }
}