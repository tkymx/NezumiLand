using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL.EventCondition {
    public struct TimeArgs {
        public float elapsedTime { get; private set; }
        public TimeArgs (string[] args) {
            Debug.Assert (args[0] != "", "第一要素がありません");
            this.elapsedTime = float.Parse (args[0]);
        }
    }

    public class Time : IEventCondition {
        float elapsedTime = 0;

        public Time (float elapsedTime) {
            this.elapsedTime = elapsedTime;
        }

        public EventConditionType EventConditionType => EventConditionType.Time;

        public List<EventConditionModel> Detect (List<EventConditionModel> eventConditionModels) {

            var outputEventConditionModels = new List<EventConditionModel> ();
            foreach (var eventConditionModel in eventConditionModels) {
                // 条件の取得
                var args = new TimeArgs (eventConditionModel.Arg);

                // 経過時間以下だったら条件を満たさない
                if (elapsedTime < args.elapsedTime) {
                    continue;
                }

                // 追加
                outputEventConditionModels.Add (eventConditionModel);
            }

            return outputEventConditionModels;
        }
    }
}