using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NL
{
    public class DebugLogger : SingletonMonoBehaviour<DebugLogger>
    {
        private readonly string LR = "\n";

        [SerializeField]
        private Text text = null;

        private void Update()
        {
            text.text = "";
            text.text += "MouseState : " + GameManager.Instance.Mouse.StateManager.CurrentState.ToString() + LR;
        }
    }
}