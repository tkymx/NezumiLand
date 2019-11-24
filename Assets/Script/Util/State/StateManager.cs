using System;
using UnityEngine;

namespace NL {
    public class StateManager {
        IState currentState = null;
        public IState CurrentState => currentState;

        public StateManager (IState currentState) {
            this.currentState = currentState;
            this.currentState.onEnter ();
        }

        public void UpdateByFrame () {
            IState nextState = this.currentState.onUpdate ();
            if (nextState != null) {
                SetState (nextState);
            }
        }

        private void SetState (IState state) {
            this.currentState.onExit ();
            this.currentState = state;
            this.currentState.onEnter ();
        }

        public void Interrupt (IState nextState) {
            SetState (nextState);
        }
    }
}