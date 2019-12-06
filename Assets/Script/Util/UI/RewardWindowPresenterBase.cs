using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public abstract class RewardWindowPresenterBase : UiWindowPresenterBase
    {
        public abstract void SetRewardAmount(IRewardAmount rewardAmount);
    }
}
