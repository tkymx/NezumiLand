using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public class MakingState : IState {
        private Mouse context;
        private PlayerArrangementTargetModel playerArrangementTargetModel;

        public MakingState (Mouse context, PlayerArrangementTargetModel playerArrangementTargetModel) {
            this.context = context;
            this.playerArrangementTargetModel = playerArrangementTargetModel;
        }

        public void onEnter () {
            context.StartMake (playerArrangementTargetModel);
        }

        public IState onUpdate () {
            context.ProgressMaking( new MakingAmount(GameManager.Instance.TimeManager.DeltaTime (), 0/*todo これ微妙*/));            
            if (context.IsFinishMaking ()) {
                context.FinishMaking (playerArrangementTargetModel);
                return new BackToHomeState (this.context);
            }

            return null;
        }

        public void onExit () {

        }
    }
}