using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  NL
{
    public class GoMonoState : IState
    {
        private AppearCharacterViewModel appearCharacterViewModel;

        public GoMonoState (AppearCharacterViewModel appearCharacterViewModel) {
            this.appearCharacterViewModel = appearCharacterViewModel;
        }

        public void onEnter () {
        }

        private bool isAlivable () {
            var playerArrangementTargetModel = appearCharacterViewModel.PlayerAppearCharacterViewModel.PlayerArrangementTargetModel;
            return ObjectComparison.Distance (appearCharacterViewModel.Position, playerArrangementTargetModel.CenterPosition) < playerArrangementTargetModel.Range;
        }

        public IState onUpdate () {
            var playerArrangementTargetModel = appearCharacterViewModel.PlayerAppearCharacterViewModel.PlayerArrangementTargetModel;
            appearCharacterViewModel.MoveTo (playerArrangementTargetModel.CenterPosition);

            // 到着したとき
            if (isAlivable ()) {
                this.appearCharacterViewModel.StartPlaying();
                return new PlayingMonoState (appearCharacterViewModel);
            }
            return null;
        }

        public void onExit () {

        }
    }    
}
