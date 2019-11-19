using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL.EventCondition
{
    public struct TimeArgs {
        public float elapsedTime { get; private set; }
        public TimeArgs(string[] args)
        {
            Debug.Assert(args[0]!="", "第一要素がありません");
            this.elapsedTime = float.Parse(args[0]);
        }
    }

    public class Time : IEventCondtion
    {
        float elapsedTime = 0;

        public Time(float elapsedTime)
        {
            this.elapsedTime = elapsedTime;            
        }

        public EventConditionType EventConditionType => EventConditionType.Time;

        public List<PlayerEventModel> Detect(List<PlayerEventModel> playerEventModels) {

            var outputPlayerEventModels = new List<PlayerEventModel>();
            foreach (var playerEventModel in playerEventModels)
            {
                bool updateFlag = false;

                for (int i = 0; i < playerEventModel.EventModel.EventConditionModels.Length; i++)
                {                    
                    var conditionModel = playerEventModel.EventModel.EventConditionModels[i];
                    if (conditionModel.EventConditionType != EventConditionType) {
                        continue;
                    }

                    // 条件の取得
                    var args = new TimeArgs(conditionModel.Arg);

                    // 経過時間の条件
                    if (elapsedTime < args.elapsedTime) {
                        continue;
                    }

                    updateFlag = true;
                    playerEventModel.ToDone(i);
                }

                // 更新があれば更新する
                if (updateFlag) {
                    outputPlayerEventModels.Add(playerEventModel);
                }
            }

            return outputPlayerEventModels;
        }
    }
}
