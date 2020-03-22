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
        private GameObject debugOptionRoot = null;

        // リポジトリ
        ParkOpenCardRepository parkOpenCardRepository;
        IPlayerParkOpenCardRepository playerParkOpenCardRepository;
        IPlayerParkOpenDeckRepository playerParkOpenDeckRepository;

        public void Initialize(
            ParkOpenCardRepository parkOpenCardRepository,
            IPlayerParkOpenCardRepository playerParkOpenCardRepository,
            IPlayerParkOpenDeckRepository playerParkOpenDeckRepository)
        {
            this.parkOpenCardRepository = parkOpenCardRepository;
            this.playerParkOpenCardRepository = playerParkOpenCardRepository;
            this.playerParkOpenDeckRepository = playerParkOpenDeckRepository;
        }

        private void Start() {

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

            this.AddDebugButton("Appear for Park Open",() => {
                var appearCharacterRepository = new AppearCharacterRepository(ContextMap.DefaultMap);
                var appearParkOpenCharacterDirectorRepository = new AppearParkOpenCharacterDirectorRepository(appearCharacterRepository, ContextMap.DefaultMap);
                var appearParkOpenCharacterDirectorModel = appearParkOpenCharacterDirectorRepository.Get(1);

                GameManager.Instance.ParkOpenAppearManager.AppearRandom(appearParkOpenCharacterDirectorModel);
                GameManager.Instance.ParkOpenAppearManager.AppearRandom(appearParkOpenCharacterDirectorModel);
                GameManager.Instance.ParkOpenAppearManager.AppearRandom(appearParkOpenCharacterDirectorModel);
            });
            
            this.AddDebugButton("ハートを増やす",()=>{
                GameManager.Instance.ParkOpenManager.AddHeart(10);
            });

            this.AddDebugButton("デッキを作る",()=>{

                // デッキがない場合は作成
                if (this.playerParkOpenDeckRepository.GetAll().Count <= 0) {
                    GameManager.Instance.ParkOpenCardManager.CreateDeck();
                }

                // カードを持っていない場合は取得
                if (this.playerParkOpenCardRepository.GetAll().Count <= 0) {
                    Debug.Assert(this.parkOpenCardRepository.GetAll().Any(), "獲得できるカードがありません");
                    GameManager.Instance.ParkOpenCardManager.ObtainCard(this.parkOpenCardRepository.GetAll().First());
                }

                // デッキに搭載
                var deck = this.playerParkOpenDeckRepository.GetAll()[0];
                var card = this.playerParkOpenCardRepository.GetAll()[0]; 
                GameManager.Instance.ParkOpenCardManager.SetCardToDeck(deck, PlayerParkOpenDeckModel.CountType.First, card);
                GameManager.Instance.ParkOpenCardManager.SetCardToDeck(deck, PlayerParkOpenDeckModel.CountType.Second, card);
                GameManager.Instance.ParkOpenCardManager.SetCardToDeck(deck, PlayerParkOpenDeckModel.CountType.Third, card);

                // メインのデッキにする
                GameManager.Instance.ParkOpenCardManager.SetMainDeck(deck);
            });

            Application.logMessageReceived += HandleLog;
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

        private void Update () {
            text.text = LR;
            text.text += "Mode : " + GameManager.Instance.GameModeManager.ToString () + LR;
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