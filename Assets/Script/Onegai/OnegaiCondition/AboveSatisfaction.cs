using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NL {
    namespace OnegaiConditions {
        public struct AboveSatisfactionArgs {
            public Satisfaction Satisfaction { get; private set; }
            public AboveSatisfactionArgs (OnegaiConditionArg args) {
                Debug.Assert (args.Args.Length >= 1, "AboveSatisfactionのArgsの数が1以上ではありません。");
                this.Satisfaction = new Satisfaction(long.Parse (args.Args[0]));
            }
            public bool IsClear(Satisfaction compareValue) {                
                return this.Satisfaction <= compareValue;
            }
        }

        public class AboveSatisfaction : IOnegaiConditionBase {
            private Satisfaction currentSatisfaction;
            public AboveSatisfaction (Satisfaction currentSatisfaction) {
                this.currentSatisfaction = currentSatisfaction;
            }

            public OnegaiCondition OnegaiCondition => NL.OnegaiCondition.AboveSatisfaction;

            public List<PlayerOnegaiModel> Mediate (List<PlayerOnegaiModel> playerOnegaiModels) {
                var outputPlayerOnegaiModels = new List<PlayerOnegaiModel> ();
                foreach (var playerOnegaiModel in playerOnegaiModels) {

                    var nearArgs = new AboveSatisfactionArgs (playerOnegaiModel.OnegaiModel.OnegaiConditionArg);
                    if (!nearArgs.IsClear(this.currentSatisfaction)) {
                        continue;
                    }

                    playerOnegaiModel.ToClear ();
                    outputPlayerOnegaiModels.Add (playerOnegaiModel);
                }

                return outputPlayerOnegaiModels;
            }
        }
    }
}