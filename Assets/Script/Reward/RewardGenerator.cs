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

            return new InvalidRewardAmount();
        }
    }
}
