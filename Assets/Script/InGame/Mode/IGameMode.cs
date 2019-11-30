using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public interface IGameMode {
        string UniqueKey();
        void OnEnter ();
        void OnUpdate ();
        void OnExit ();
    }
}