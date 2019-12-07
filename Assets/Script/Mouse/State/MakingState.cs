using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public class MakingState : IState {
        private Mouse context;
        private IArrangementTarget arrangementTarget;

        public MakingState (Mouse context, IArrangementTarget makingPosition) {
            this.context = context;
            this.arrangementTarget = makingPosition;
        }

        public void onEnter () {
            context.StartMake (arrangementTarget);
        }

        public IState onUpdate () {
            context.ProgressMaking( new MakingAmount(GameManager.Instance.TimeManager.DeltaTime (), 0/*todo これ微妙*/));            
            if (context.IsFinishMaking ()) {
                context.FinishMaking (arrangementTarget);
                return new BackToHomeState (this.context);
            }

            return null;
        }

        public void onExit () {

        }
    }
}