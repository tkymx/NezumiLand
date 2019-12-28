using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NL {
    namespace OnegaiConditions {
        public struct ArrangementCountArgs {
            public uint MonoId { get; private set; }
            public uint Count { get; private set; }
            public ArrangementCountArgs (OnegaiConditionArg args) {
                Debug.Assert (args.Args.Length >= 2, "ArrangementCountのArgsの数が2以上ではありません。");
                this.MonoId = uint.Parse (args.Args[0]);
                this.Count = uint.Parse (args.Args[1]);
            }
            public bool IsClear(uint monoId, uint count) {                
                if (this.MonoId != monoId) {
                    return false;
                }
                return this.Count <= count;
            }
        }

        public class ArrangementCount : IOnegaiConditionBase {
            private uint monoId;
            private uint count;
            public ArrangementCount (uint monoId, uint count) {
                this.monoId = monoId;
                this.count = count;
            }

            public OnegaiCondition OnegaiCondition => NL.OnegaiCondition.ArrangementCount;

            public List<PlayerOnegaiModel> Mediate (List<PlayerOnegaiModel> playerOnegaiModels) {
                var outputPlayerOnegaiModels = new List<PlayerOnegaiModel> ();
                foreach (var playerOnegaiModel in playerOnegaiModels) {

                    var nearArgs = new ArrangementCountArgs (playerOnegaiModel.OnegaiModel.OnegaiConditionArg);
                    if (!nearArgs.IsClear(this.monoId, this.count)) {
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