using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NL {
    public struct NearArgs {
        public uint NearTargetMonoInfoId { get; private set; }
        public NearArgs (OnegaiConditionArg args) {
            Debug.Assert (args.Args.Length == 1, "NearのArgsの数が1ではありません。");
            this.NearTargetMonoInfoId = uint.Parse (args.Args[0]);
        }
    }

    public class Near : IOnegaiConditionBase {
        private List<uint> nearMonoInfoIDs;
        public Near (List<uint> nearMonoInfoIDs) {
            this.nearMonoInfoIDs = nearMonoInfoIDs;
        }

        public OnegaiCondition OnegaiCondition => OnegaiCondition.Near;

        public List<PlayerOnegaiModel> Mediate (List<PlayerOnegaiModel> playerOnegaiModels) {
            var outputPlayerOnegaiModels = new List<PlayerOnegaiModel> ();
            foreach (var playerOnegaiModel in playerOnegaiModels) {
                // 隣接モノが条件に合うかどうか？
                var nearArgs = new NearArgs (playerOnegaiModel.OnegaiModel.OnegaiConditionArg);
                if (nearMonoInfoIDs.All (id => nearArgs.NearTargetMonoInfoId != id)) {
                    continue;
                }

                // クリアにする
                playerOnegaiModel.ToClear ();
                outputPlayerOnegaiModels.Add (playerOnegaiModel);
            }

            return outputPlayerOnegaiModels;
        }
    }
}