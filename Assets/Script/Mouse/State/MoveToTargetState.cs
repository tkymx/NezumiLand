using System.Collections;
using UnityEngine;

namespace NL {
    public class MoveToTarget : IState {
        private Mouse context;
        private PlayerArrangementTargetModel playerArrangementTargetModel;

        public MoveToTarget (Mouse context, PlayerArrangementTargetModel playerArrangementTargetModel) {
            this.context = context;
            this.playerArrangementTargetModel = playerArrangementTargetModel;
        }

        public void onEnter () {

        }

        private bool isAlivable () {
            return ObjectComparison.Distance (context.transform.position, playerArrangementTargetModel.CenterPosition) < playerArrangementTargetModel.Range;
        }

        public IState onUpdate () {
            context.MoveTimeTo (playerArrangementTargetModel.CenterPosition);

            // 到着したとき
            if (isAlivable ()) {
                // 物があれば作成する
                if (context.HasPreMono) {
                    return new MakingState (context, playerArrangementTargetModel);
                }

                return new EmptyState ();
            }
            return null;
        }

        public void onExit () {

        }
    }
}