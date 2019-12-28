using System;
using UnityEngine;

namespace NL {
    public class StateManager {
        IState currentState = null;
        public IState CurrentState => currentState;

        public TypeObservable<IState> OnChangeStateObservable { get; private set;}

        public StateManager (IState currentState) {
            this.OnChangeStateObservable = new TypeObservable<IState>();
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
            this.OnChangeStateObservable.Execute(state);
            this.currentState.onEnter ();
        }

        public void Interrupt (IState nextState) {
            SetState (nextState);
        }
    }
}