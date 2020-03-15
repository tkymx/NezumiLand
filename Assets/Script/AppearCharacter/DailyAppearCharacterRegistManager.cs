using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {

    /// <summary>
    /// 毎日必要なキャラクタを表示する
    /// 基本的に条件に合うものをレジストしていく。レジストする必要がなくなったら候補から消す
    /// </summary>
    public class DailyAppearCharacterRegistManager
    {
        private readonly DailyAppearCharacterRegistReserveCreateService dailyAppearCharacterRegistReserveCreateService = null;
        private readonly DailyAppearCharacterRegistReserveRemoveService dailyAppearCharacterRegistReserveRemoveService = null;
        private readonly DailyAppearCharacterRegistReserveNextRemoveService dailyAppearCharacterRegistReserveNextRemoveService = null;
        private readonly DailyAppearCharacterRegistReserveNextSkipService dailyAppearCharacterRegistReserveNextSkipService = null;

        private List<DailyAppearCharacterGeneratorResistReserve> dailyAppearCharacterGeneratorResistReserves = null;

        public DailyAppearCharacterRegistManager(IPlayerAppearCharacterReserveRepository playerAppearCharacterReserveRepository)
        {
            this.dailyAppearCharacterGeneratorResistReserves = new List<DailyAppearCharacterGeneratorResistReserve>();            

            this.dailyAppearCharacterRegistReserveCreateService = new DailyAppearCharacterRegistReserveCreateService(playerAppearCharacterReserveRepository);
            this.dailyAppearCharacterRegistReserveRemoveService = new DailyAppearCharacterRegistReserveRemoveService(playerAppearCharacterReserveRepository);
            this.dailyAppearCharacterRegistReserveNextRemoveService = new DailyAppearCharacterRegistReserveNextRemoveService(playerAppearCharacterReserveRepository);
            this.dailyAppearCharacterRegistReserveNextSkipService = new DailyAppearCharacterRegistReserveNextSkipService(playerAppearCharacterReserveRepository);
        }

        public void RegistReserve(
            AppearCharacterLifeDirectorType appearCharacterLifeDirectorType, 
            AppearCharacterDirectorModelBase appearCharacterDirectorModelBase, 
            IDailyAppearCharacterRegistCondition dailyAppearCharacterRegistCondition) 
        {
            var playerAppearCharacterReserveModel = this.dailyAppearCharacterRegistReserveCreateService.Execute(
                appearCharacterLifeDirectorType,
                appearCharacterDirectorModelBase,
                dailyAppearCharacterRegistCondition
            );
            this.RegistReserve(playerAppearCharacterReserveModel);
        }
        public void RegistReserve(PlayerAppearCharacterReserveModel playerAppearCharacterReserveModel) 
        {
            var dailyAppearCharacterGeneratorResistReserve = new DailyAppearCharacterGeneratorResistReserve(playerAppearCharacterReserveModel);
            this.dailyAppearCharacterGeneratorResistReserves.Add(dailyAppearCharacterGeneratorResistReserve);
        }

        public bool IsRemoveReserve(PlayerAppearCharacterReserveModel playerAppearCharacterReserveModel)
        {
            var dailyAppearCharacterGeneratorResistReserve = this.dailyAppearCharacterGeneratorResistReserves.Find(reserve => reserve.PlayerAppearCharacterReserveModel.Id == playerAppearCharacterReserveModel.Id);
            Debug.Assert(dailyAppearCharacterGeneratorResistReserve != null, "要素が見つかりません");

            // 条件は増える可能性があるが、現状一回のみのやつは消してもいい
            return dailyAppearCharacterGeneratorResistReserve.IsOnce();
        }

        public void RemoveReserve(PlayerAppearCharacterReserveModel playerAppearCharacterReserveModel)
        {
            var dailyAppearCharacterGeneratorResistReserve = this.dailyAppearCharacterGeneratorResistReserves.Find(reserve => reserve.PlayerAppearCharacterReserveModel.Id == playerAppearCharacterReserveModel.Id);
            Debug.Assert(dailyAppearCharacterGeneratorResistReserve != null, "DailyAppearCharacterRegistManagerに見つかりませんでした。");

            // 消去予約
            this.dailyAppearCharacterRegistReserveNextRemoveService.Execute(dailyAppearCharacterGeneratorResistReserve.PlayerAppearCharacterReserveModel);
        }

        public void SetReserveNextSkippable(PlayerAppearCharacterReserveModel playerAppearCharacterReserveModel, bool isNextSkip)
        {
            var dailyAppearCharacterGeneratorResistReserve = this.dailyAppearCharacterGeneratorResistReserves.Find(reserve => reserve.PlayerAppearCharacterReserveModel.Id == playerAppearCharacterReserveModel.Id);
            Debug.Assert(dailyAppearCharacterGeneratorResistReserve != null, "DailyAppearCharacterRegistManagerに見つかりませんでした。");

            // スキップ予約
            this.dailyAppearCharacterRegistReserveNextSkipService.Execute(dailyAppearCharacterGeneratorResistReserve.PlayerAppearCharacterReserveModel, isNextSkip);
        }        

        public void Regist() 
        {
            // 消去すべきかを判断する
            foreach (var dailyAppearCharacterGeneratorResistReserve in dailyAppearCharacterGeneratorResistReserves)
            {
                if (!dailyAppearCharacterGeneratorResistReserve.PlayerAppearCharacterReserveModel.IsNextRemove) {
                    continue;
                }

                this.dailyAppearCharacterGeneratorResistReserves.Remove(dailyAppearCharacterGeneratorResistReserve);
                this.dailyAppearCharacterRegistReserveRemoveService.Execute(dailyAppearCharacterGeneratorResistReserve.PlayerAppearCharacterReserveModel);
            }

            foreach (var dailyAppearCharacterGeneratorResistReserve in dailyAppearCharacterGeneratorResistReserves)
            {
                // 条件が満たしていたら
                if (!dailyAppearCharacterGeneratorResistReserve.IsResist()) {
                    continue;
                }

                if (dailyAppearCharacterGeneratorResistReserve.PlayerAppearCharacterReserveModel.IsNextSkip) {
                    continue;
                }

                // キャラクタを生成する
                dailyAppearCharacterGeneratorResistReserve.Generate();
            }
        }
    }
}