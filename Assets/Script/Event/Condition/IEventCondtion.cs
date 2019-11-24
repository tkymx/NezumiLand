using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public interface IEventCondtion {
        /// <summary>
        /// 自分のイベント条件のタイプを返す
        /// </summary>
        /// <value></value>
        EventConditionType EventConditionType { get; }

        /// <summary>
        /// イベントに対して条件の判定を行って、もし達成していたら達成した条件を返す
        /// </summary>
        /// <param name="targetEventConditionModel"></param>
        /// <returns></returns>
        List<EventConditionModel> Detect (List<EventConditionModel> targetEventConditionModel);
    }
}