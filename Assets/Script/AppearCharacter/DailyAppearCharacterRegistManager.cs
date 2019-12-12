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
        private List<DailyAppearCharacterGeneratorResistReserve> dailyAppearCharacterGeneratorResistReserves = null;

        public DailyAppearCharacterRegistManager()
        {
            this.dailyAppearCharacterGeneratorResistReserves = new List<DailyAppearCharacterGeneratorResistReserve>();            
        }

        public void RegistReserve(DailyAppearCharacterGeneratorResistReserve dailyAppearCharacterGeneratorResistReserve) 
        {
            Debug.Log("キャラクタの出演予約をしました。:" + dailyAppearCharacterGeneratorResistReserve.ToString());
            this.dailyAppearCharacterGeneratorResistReserves.Add(dailyAppearCharacterGeneratorResistReserve);
        }

        public bool IsRemoveReserve(AppearCharacterViewModel appearCharacterViewModel)
        {
            var dailyAppearCharacterGeneratorResistReserve = this.dailyAppearCharacterGeneratorResistReserves.Find(reserve => reserve.IsTarget(appearCharacterViewModel));
            return dailyAppearCharacterGeneratorResistReserve != null;
        }

        public void RemoveReserve(AppearCharacterViewModel appearCharacterViewModel)
        {
            var dailyAppearCharacterGeneratorResistReserve = this.dailyAppearCharacterGeneratorResistReserves.Find(reserve => reserve.IsTarget(appearCharacterViewModel));
            Debug.Assert(dailyAppearCharacterGeneratorResistReserve != null, "DailyAppearCharacterRegistManagerに見つかりませんでした。");
            this.dailyAppearCharacterGeneratorResistReserves.Remove(dailyAppearCharacterGeneratorResistReserve);
        }

        public void Resist() 
        {
            var removeList = new Queue<DailyAppearCharacterGeneratorResistReserve>();
            foreach (var dailyAppearCharacterGeneratorResistReserve in dailyAppearCharacterGeneratorResistReserves)
            {
                // 条件が満たしていたら
                if (!dailyAppearCharacterGeneratorResistReserve.IsResist()) {
                    continue;
                }

                // キャラクタを生成する
                dailyAppearCharacterGeneratorResistReserve.Generate();

                // これで終わりの場合は消去リストに入れる
                if (dailyAppearCharacterGeneratorResistReserve.IsRemove()) {
                    removeList.Enqueue(dailyAppearCharacterGeneratorResistReserve);                
                }
            }
            while(removeList.Count > 0) {
                dailyAppearCharacterGeneratorResistReserves.Remove(removeList.Dequeue());
            }
        }
    }
}