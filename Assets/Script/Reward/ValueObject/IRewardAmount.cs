using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {

    public enum RewardType {
        None,
        Currency,
        Item,
        Onegai,
        Mono
    }

    public interface IRewardAmount
    {
        string Name { get; }
        RewardType RewardType { get; }
        uint Amount { get; }
        Sprite Image { get; }
        void Receive();
    }
}