using System;

namespace NL {
    public interface IState {
        void onEnter ();
        IState onUpdate ();
        void onExit ();
    }
}