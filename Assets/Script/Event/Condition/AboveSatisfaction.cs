using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL.EventCondition
{
    public struct SatisfactionArgs {
        public Satisfaction Satisfaction { get; private set; }
        public SatisfactionArgs(string[] args)
        {
            Debug.Assert(args[0]!="", "第一要素がありません");
            this.Satisfaction = new Satisfaction(long.Parse(args[0]));
        }
    }

    public class AboveSatisfaction : IEventCondtion
    {
        Satisfaction currentSatisfaction = new Satisfaction(0);

        public AboveSatisfaction(Satisfaction currentSatisfaction)
        {
            this.currentSatisfaction = currentSatisfaction;            
        }

        public EventConditionType EventConditionType => EventConditionType.AboveSatisfaction;

        public List<EventConditionModel> Detect(List<EventConditionModel> eventConditionModels) {

            var outputEventConditionModels = new List<EventConditionModel>();
            foreach (var eventConditionModel in outputEventConditionModels)
            {
                // 条件の取得
                var args = new SatisfactionArgs(eventConditionModel.Arg);

                // 一定満足度以下だったら条件を満たさない
                if (currentSatisfaction < args.Satisfaction) {
                    continue;
                }

                // 追加
                outputEventConditionModels.Add(eventConditionModel);
            }

            return outputEventConditionModels;
        }
    }
}
