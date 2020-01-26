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
        private Button dailyAppearCharacterRegistButton = null;

        [SerializeField]
        private Button appearForParkOpen = null;

        [SerializeField]
        private Button parkOpenButton = null;

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
                
                #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
                #else
                    Application.Quit();
                #endif                    
            });
            dailyAppearCharacterRegistButton.onClick.AddListener(()=>{
                GameManager.Instance.DailyAppearCharacterRegistManager.Regist();
            });
            openCloseButton.onClick.AddListener(() => {
                mainContetns.SetActive(!mainContetns.activeSelf);
            });
            appearForParkOpen.onClick.AddListener(() => {
                var appearCharacterRepository = new AppearCharacterRepository(ContextMap.DefaultMap);
                var appearCharacterModel = appearCharacterRepository.Get(1);

                GameManager.Instance.ParkOpenAppearManager.AppearRandom(appearCharacterModel);
                GameManager.Instance.ParkOpenAppearManager.AppearRandom(appearCharacterModel);
                GameManager.Instance.ParkOpenAppearManager.AppearRandom(appearCharacterModel);
            });
            parkOpenButton.onClick.AddListener(() => {
                var appearCharacterRepository = new AppearCharacterRepository(ContextMap.DefaultMap);
                var parkOpenWaveRepository = new ParkOpenWaveRepository(appearCharacterRepository, ContextMap.DefaultMap);
                var parkOpenGroupRepository = new ParkOpenGroupRepository(parkOpenWaveRepository, ContextMap.DefaultMap);

                GameManager.Instance.ParkOpenManager.Open(parkOpenGroupRepository.Get(1));
            });
            Application.logMessageReceived += HandleLog;
        }

        private void Update () {
            text.text = "";
            text.text += "EventContents : " + GameManager.Instance.EventManager.CurrentEventContents.ToString () + LR;
            text.text += GameManager.Instance.ParkOpenManager.ToString() + LR;
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