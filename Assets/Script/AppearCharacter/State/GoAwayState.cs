using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  NL
{
    public class GoAwayState : IState
    {
        private readonly float ArraiveRange = 1.0f;

        private AppearCharacterViewModel appearCharacterViewModel;
        private Vector3 awayTarget;

        public GoAwayState (AppearCharacterViewModel appearCharacterViewModel) {
            this.appearCharacterViewModel = appearCharacterViewModel;
            this.awayTarget = appearCharacterViewModel.DisappearPosition;
        }

        public void onEnter () {

        }

        private bool isAlivable () {
            return ObjectComparison.Distance (appearCharacterViewModel.Position, awayTarget) < ArraiveRange;
        }

        public IState onUpdate () {
            appearCharacterViewModel.MoveTo (awayTarget);

            // 到着したとき
            if (isAlivable ()) {
                // 消去
                GameManager.Instance.AppearCharacterManager.Remove(appearCharacterViewModel);
                return new RemovedState ();
            }
            return null;
        }

        public void onExit () {

        }
    }    
}
