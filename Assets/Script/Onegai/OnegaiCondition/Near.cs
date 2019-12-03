using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NL {
    public struct NearArgs {
        public uint TargetMonoInfoId { get; private set; }
        public uint NearMonoInfoId { get; private set; }
        public NearArgs (OnegaiConditionArg args) {
            Debug.Assert (args.Args.Length >= 2, "NearのArgsの数が2以上ではありません。");
            this.TargetMonoInfoId = uint.Parse (args.Args[0]);
            this.NearMonoInfoId = uint.Parse (args.Args[1]);
        }
        public bool IsClear(uint targetMonoInfoId, List<uint> nearMonoInfoIDs) {
            
            if (this.TargetMonoInfoId != targetMonoInfoId) {
                return false;
            }

            var nearMonoInfoId = this.NearMonoInfoId;
            if (nearMonoInfoIDs.All(id => id != nearMonoInfoId)) {
                return false;
            }
            return true;
        }
    }

    public class Near : IOnegaiConditionBase {
        private uint targetMonoInfoID;
        private List<uint> nearMonoInfoIDs;
        public Near (uint targetMonoInfoID, List<uint> nearMonoInfoIDs) {
            this.targetMonoInfoID = targetMonoInfoID;
            this.nearMonoInfoIDs = nearMonoInfoIDs;
        }

        public OnegaiCondition OnegaiCondition => OnegaiCondition.Near;

        public List<PlayerOnegaiModel> Mediate (List<PlayerOnegaiModel> playerOnegaiModels) {
            var outputPlayerOnegaiModels = new List<PlayerOnegaiModel> ();
            foreach (var playerOnegaiModel in playerOnegaiModels) {

                var nearArgs = new NearArgs (playerOnegaiModel.OnegaiModel.OnegaiConditionArg);
                if (!nearArgs.IsClear(this.targetMonoInfoID, this.nearMonoInfoIDs)) {
                    continue;
                }

                playerOnegaiModel.ToClear ();
                outputPlayerOnegaiModels.Add (playerOnegaiModel);
            }

            return outputPlayerOnegaiModels;
        }
    }
}