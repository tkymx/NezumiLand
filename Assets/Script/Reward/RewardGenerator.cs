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
            
            Debug.Assert(false, "無効な報酬が設定されいています。 : " + type);
            return new InvalidRewardAmount();
        }

        public static IRewardAmount InValidReward => new InvalidRewardAmount();
    }
}
