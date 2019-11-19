using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public interface IEventCondtion
    {
        /// <summary>
        /// 自分のイベント条件のタイプを返す
        /// </summary>
        /// <value></value>
        EventConditionType EventConditionType { get; }

        /// <summary>
        /// イベントに対して条件の判定を行って、もし変化があれば返却する
        /// </summary>
        /// <param name="targetEventConditionModel"></param>
        /// <returns></returns>
        List<PlayerEventModel> Detect(List<PlayerEventModel> targetEventConditionModel);
    }
}
