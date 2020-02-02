using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

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
        private Button addHeartButton = null;

        [SerializeField]
        private Button createKariDeck = null;

        [SerializeField]
        private GameObject mainContetns = null;

        [SerializeField]
        private Text text = null;

        [SerializeField]
        private Button removePlayerData = null;

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

                // 開始
                GameManager.Instance.ParkOpenManager.Open(parkOpenGroupRepository.Get(1));
            });
            addHeartButton.onClick.AddListener(()=>{
                GameManager.Instance.ParkOpenManager.AddHeart(10);
            });
            createKariDeck.onClick.AddListener(()=>{

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