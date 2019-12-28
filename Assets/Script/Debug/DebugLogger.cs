using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NL {
    public class DebugLogger : SingletonMonoBehaviour<DebugLogger> {
        private readonly string LR = "\n";

        [SerializeField]
        private Button openCloseButton = null;

        [SerializeField]
        private GameObject mainContetns = null;

        [SerializeField]
        private Text text = null;

        [SerializeField]
        private Button removePlayerData = null;

        private void Start() {
            removePlayerData.onClick.AddListener(()=>{
                ResourceLoader.RemoveAllPlayerData();
                GameManager.Instance.GameUIManager.CommonPresenter.SetContents("Debug","プレイヤーデータを消去しました。");
                GameManager.Instance.GameUIManager.CommonPresenter.Show();
            });
            openCloseButton.onClick.AddListener(() => {
                mainContetns.SetActive(!mainContetns.activeSelf);
            });
            Application.logMessageReceived += HandleLog;
        }

        private void Update () {
            text.text = "";
            text.text += "EventContents : " + GameManager.Instance.EventManager.CurrentEventContents.ToString () + LR;
            if (GameManager.Instance.MonoSelectManager.HasSelectedMonoInfo) {
                text.text += "MonoState : " + GameManager.Instance.MonoSelectManager.SelectedMonoInfo.Id.ToString () + ":" + GameManager.Instance.MonoSelectManager.SelectedMonoInfo.Name.ToString () + LR;
            }
            text.text += "Mode : " + GameManager.Instance.GameModeManager.ToString () + LR;
            text.text += message;
        }

        private string message = "";

        private void OnDestroy()
        {
            Application.logMessageReceived -= HandleLog;
        }

        private void HandleLog( string logText, string stackTrace, LogType type )
        {
            message += type.ToString() + ":" + logText + LR;
        }        
    }
}