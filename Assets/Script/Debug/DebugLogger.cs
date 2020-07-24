using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using UnityEngine.Events;

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
        private GameObject debugButtonPrefab = null;

        [SerializeField]
        private GameObject debugInputButtonPrefab = null;

        [SerializeField]
        private GameObject debugOptionRoot = null;

        public void Initialize()
        {
        }

        private void Start() {

#if DEVELOPMENT_BUILD || UNITY_EDITOR

            openCloseButton.onClick.AddListener(() => {
                mainContetns.SetActive(!mainContetns.activeSelf);
            });

            this.AddDebugButton("rm playerData", ()=>{
                ResourceLoader.RemoveAllPlayerData();
                GameManager.Instance.GameUIManager.CommonPresenter.SetContents("Debug","プレイヤーデータを消去しました。");
                GameManager.Instance.GameUIManager.CommonPresenter.Show();
                
                #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
                #else
                    Application.Quit();
                #endif                    
            });
            
            this.AddDebugButton("Daily Appear Character Resist", ()=>{
                GameManager.Instance.AppearCharacterManager.RemoveAllSoon();
                GameManager.Instance.DailyAppearCharacterRegistManager.Regist();
            });
            
            this.AddDebugInputButton("カメラ回転ファクター", value => {
                var factor = float.Parse(value);
                GameManager.Instance.GlobalSystemParameter.OverrideRotationMoveFactor(factor);
            });

            this.AddDebugInputButton("カメラピンチファクター", value => {
                var factor = float.Parse(value);
                GameManager.Instance.GlobalSystemParameter.OverridePinchMoveFactor(factor);
            });

            this.AddDebugInputButton("カメラピンチNear", value => {
                var factor = float.Parse(value);
                GameManager.Instance.GlobalSystemParameter.OverridePinchNearLength(factor);
            });

            this.AddDebugInputButton("カメラピンチFar", value => {
                var factor = float.Parse(value);
                GameManager.Instance.GlobalSystemParameter.OverridePinchFarLength(factor);
            });

            Application.logMessageReceived += HandleLog;

#else
            openCloseButton.gameObject.SetActive(false);
#endif

        }

        private void AddDebugButton(string title, UnityAction onCLick)
        {
            var instance = GameObject.Instantiate(this.debugButtonPrefab);
            instance.transform.SetParent(this.debugOptionRoot.transform, false);
            var button = instance.GetComponent<Button>();
            Debug.Assert(button!=null, "ボタンが設定されていません");
            var text = instance.GetComponentInChildren<Text>();
            Debug.Assert(text!=null, "テキストが存在しません");
            text.text = title;
            button.onClick.AddListener(onCLick);
        }

        private void AddDebugInputButton(string title, UnityAction<string> onCLick)
        {
            var instance = GameObject.Instantiate(this.debugInputButtonPrefab);
            instance.transform.SetParent(this.debugOptionRoot.transform, false);
            var button = instance.GetComponentInChildren<Button>();
            Debug.Assert(button!=null, "ボタンが設定されていません");
            var text = instance.GetComponentInChildren<Text>();
            Debug.Assert(text!=null, "テキストが存在しません");
            text.text = title;
            var input = instance.GetComponentInChildren<InputField>();
            Debug.Assert(input!=null, "インプットが存在しません");
            button.onClick.AddListener(() => {
                onCLick(input.text);
            });
        }        

        private void Update () {
            text.text = LR;
            text.text += "Mode : " + GameManager.Instance.GameModeManager.ToString () + LR;
            text.text += "Input.touchCount : " + Input.touchCount.ToString() + LR;
            text.text += string.Join(LR,message);
        }

        private List<string> message = new List<string>();

        private void OnDestroy()
        {
            Application.logMessageReceived -= HandleLog;
        }

        private void HandleLog( string logText, string stackTrace, LogType type )
        {
            if (message.Count > 50) {
                message.RemoveAt(0);
            }
            message.Add(type.ToString() + ":" + logText);
        }        
    }
}