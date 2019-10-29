using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public interface IGameMode
    {
        void OnEnter();
        void OnUpdate();
        void OnExit();
    }
}