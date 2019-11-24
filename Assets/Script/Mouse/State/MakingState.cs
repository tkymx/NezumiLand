using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public class MakingState : IState {
        private Mouse context;
        private IArrangementTarget arrangementTarget;

        private float elapsedTime = 0;

        public MakingState (Mouse context, IArrangementTarget makingPosition) {
            this.context = context;
            this.arrangementTarget = makingPosition;
            this.elapsedTime = 0;
        }

        public void onEnter () {
            context.StartMake (arrangementTarget);
        }

        public IState onUpdate () {
            elapsedTime += GameManager.Instance.TimeManager.DeltaTime ();
            if (elapsedTime > 2.0f) {
                context.FinishMaking (arrangementTarget);
                return new BackToHomeState (this.context);
            }

            return null;
        }

        public void onExit () {

        }
    }
}