using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public interface IOnegaiConditionBase
    {
        OnegaiCondition OnegaiCondition { get; }

        /// <summary>
        /// モデルが条件に合うかを判断して判断によって変化のあったものを返却する
        /// </summary>
        /// <param name="playerOnegaiModels">変化のあったモデル</param>
        /// <returns></returns>
        List<PlayerOnegaiModel> Mediate(List<PlayerOnegaiModel> playerOnegaiModels);
    }
}
