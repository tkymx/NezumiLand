using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NL {
    public class DebugLogger : SingletonMonoBehaviour<DebugLogger> {
        private readonly string LR = "\n";

        [SerializeField]
        private Text text = null;

        private void Update () {
            text.text = "";
            /*
                        if (GameManager.Instance.MouseSelectManager.HasSelectedMouse)
                        {
                            text.text += "MouseState : " + GameManager.Instance.MouseSelectManager.SelectedMouse.StateManager.CurrentState.ToString() + LR;
                        }
            */
            if (GameManager.Instance.MonoSelectManager.HasSelectedMonoInfo) {
                text.text += "MonoState : " + GameManager.Instance.MonoSelectManager.SelectedMonoInfo.Id.ToString () + ":" + GameManager.Instance.MonoSelectManager.SelectedMonoInfo.Name.ToString () + LR;
            }
            text.text += "Mode : " + GameManager.Instance.GameModeManager.ToString () + LR;
        }
    }
}