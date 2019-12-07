using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace NL {
    public class RewardGenerator
    {
        public static IRewardAmount Generate(string type, uint amount, string[] args) {
            
            // タイプの取得
            var rewardType = RewardType.None;
            if (Enum.TryParse (type, out RewardType outRewardType)) {
                rewardType = outRewardType;
            }

            if (rewardType == RewardType.Currency) {
                return new CurrencyRewardAmount(amount);
            }

            if (rewardType == RewardType.Item) {
                return new ItemRewardAmount(amount);
            }

            if (rewardType == RewardType.Onegai) {
                Debug.Assert(args.Length >= 1, "RewardType.Onegai の要素数が足りません");
                uint onegaiId = 0;
                if (args.Length >= 1) {
                    onegaiId = uint.Parse(args[0]);
                }
                return new OnegaiRewardAmount(amount, onegaiId);
            }

            if (rewardType == RewardType.Mono) {
                Debug.Assert(args.Length >= 1, "RewardType.Mono の要素数が足りません");
                uint monoInfoId = 0;
                if (args.Length >= 1) {
                    monoInfoId = uint.Parse(args[0]);
                }
                return new MonoRewardAmount(amount, monoInfoId);
            }            
            
            Debug.Assert(false, "無効な報酬が設定されいています。 : " + type);
            return new InvalidRewardAmount();
        }

        public static IRewardAmount InValidReward => new InvalidRewardAmount();
    }
}
