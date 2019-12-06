using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public class GameManager : SingletonMonoBehaviour<GameManager> {
        [SerializeField]
        private Camera mainCamera = null;
        public Camera MainCamera => mainCamera;

        public GameObject rootObject;
        public GameObject rootEffectUI;

        [SerializeField]
        private ArrangementPresenter arrangementPresenter = null;
        public ArrangementPresenter ArrangementPresenter => arrangementPresenter;

        [SerializeField]
        private GameUIManager gameUIManager = null;
        public GameUIManager GameUIManager => gameUIManager;

        private ArrangementManager arrangementManager;
        public ArrangementManager ArrangementManager => arrangementManager;

        private MonoManager monoManager;
        public MonoManager MonoManager => monoManager;

        private Wallet wallet;
        public Wallet Wallet => wallet;

        private EffectManager effectManager;
        public EffectManager EffectManager => effectManager;

        private GameModeManager gameModeManager;
        public GameModeManager GameModeManager => gameModeManager;

        private FieldRaycastManager fieldRaycastManager;
        public FieldRaycastManager FieldRaycastManager => fieldRaycastManager;

        // 使用意図から外れたので一旦消す
        //        private MouseSelectManager mouseSelectManager;
        //        public MouseSelectManager MouseSelectManager => mouseSelectManager;

        private MonoSelectManager monoSelectManager;
        public MonoSelectManager MonoSelectManager => monoSelectManager;

        private TimeManager timeManager;
        public TimeManager TimeManager => timeManager;

        private MouseHomeManager mouseHomeManager;
        public MouseHomeManager MouseHomeManager => mouseHomeManager;

        private OnegaiHomeManager onegaiHomeManager;
        public OnegaiHomeManager OnegaiHomeManager => onegaiHomeManager;

        private MouseStockManager mouseStockManager;
        public MouseStockManager MouseStockManager => mouseStockManager;

        private DailyActionManager dailyActionManager;
        public DailyActionManager DailyActionManager => dailyActionManager;

        private ArrangementItemStore arrangementItemStore;
        public ArrangementItemStore ArrangementItemStore => arrangementItemStore;

        private EventManager eventManager;
        public EventManager EventManager => eventManager;

        private ConstantlyEventPusher constantlyEventPusher;
        public ConstantlyEventPusher ConstantlyEventPusher => constantlyEventPusher;

        private AppearCharacterManager appearCharacterManager;
        public  AppearCharacterManager AppearCharacterManager => appearCharacterManager;

        private DailyAppearCharacterRegistManager dailyAppearCharacterRegistManager;
        public DailyAppearCharacterRegistManager DailyAppearCharacterRegistManager => dailyAppearCharacterRegistManager;

        private OnegaiMediaterManager onegaiMediaterManager;
        public OnegaiMediaterManager OnegaiMediaterManager => onegaiMediaterManager;

        private OnegaiManager onegaiManager;
        public OnegaiManager OnegaiManager => onegaiManager;

        private void Start () {
            // コンテキストマップ
            ContextMap.Initialize ();
            PlayerContextMap.Initialize ();
            GameContextMap.Initialize ();

            // レポジトリ
            var onegaiRepository = new OnegaiRepository(ContextMap.DefaultMap);
            var playerOnegaiRepository = PlayerOnegaiRepository.GetRepository(ContextMap.DefaultMap, PlayerContextMap.DefaultMap);
            var playerEventRepository = PlayerEventRepository.GetRepository(ContextMap.DefaultMap, PlayerContextMap.DefaultMap);

            // instance
            this.wallet = new Wallet (new Currency (100)); // 所持金の初期値も外出ししたい
            this.arrangementItemStore = new ArrangementItemStore (new ArrangementItemAmount (100)); // 所持アイテムの初期値も外出ししたい
            this.arrangementManager = new ArrangementManager (this.rootObject);
            this.monoManager = new MonoManager (this.rootObject);
            this.effectManager = new EffectManager (mainCamera, rootEffectUI);
            this.gameModeManager = new GameModeManager ();
            this.gameModeManager.EnqueueChangeModeWithHistory (GameModeGenerator.GenerateSelectMode ());
            this.fieldRaycastManager = new FieldRaycastManager (this.mainCamera);
            //            this.mouseSelectManager = new MouseSelectManager();
            this.monoSelectManager = new MonoSelectManager ();
            this.timeManager = new TimeManager ();
            this.mouseHomeManager = new MouseHomeManager (this.rootObject);
            this.onegaiHomeManager = new OnegaiHomeManager (this.rootObject);
            this.mouseStockManager = new MouseStockManager (this.rootObject);
            this.dailyActionManager = new DailyActionManager (playerOnegaiRepository);
            this.eventManager = new EventManager(playerEventRepository);
            this.constantlyEventPusher = new ConstantlyEventPusher(playerOnegaiRepository);
            this.appearCharacterManager = new AppearCharacterManager(this.rootObject);
            this.dailyAppearCharacterRegistManager = new DailyAppearCharacterRegistManager();
            this.onegaiMediaterManager = new OnegaiMediaterManager(playerOnegaiRepository);
            this.onegaiManager = new OnegaiManager(playerOnegaiRepository);

            // initialize
            this.gameUIManager.Initialize (onegaiRepository, playerOnegaiRepository);
            this.mouseHomeManager.Initialize ();
            this.onegaiHomeManager.Initialize ();

            // Service
            OnegaiUnLockService onegaiUnLockService = new OnegaiUnLockService(onegaiRepository, playerOnegaiRepository);
            onegaiUnLockService.Execute();
            OnegaiUnLockChacheService onegaiUnLockChacheService = new OnegaiUnLockChacheService(playerOnegaiRepository);
            onegaiUnLockChacheService.Execute();
        }

        private void Update () {
            this.dailyActionManager.UpdateByFrame ();
            this.gameModeManager.UpdateByFrame ();
            this.monoManager.UpdateByFrame ();
            this.fieldRaycastManager.UpdateByFrame ();
            this.timeManager.UpdateByFrame ();
            this.appearCharacterManager.UpdateByFrame();
            this.onegaiManager.UpdateByFrame();

            // イベント関連
            this.constantlyEventPusher.PushConstantlyEventParameter();
            this.eventManager.UpdateByFrame ();
        }
    }
}