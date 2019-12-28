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

        private MonoReleaseManager monoReleaseManager;
        public MonoReleaseManager MonoReleaseManager => monoReleaseManager;

        private ReserveAmountManager reserveAmountManager;
        public ReserveAmountManager ReserveAmountManager => reserveAmountManager;

        private void Start () {
            // コンテキストマップ
            ContextMap.Initialize ();
            PlayerContextMap.Initialize ();

            // レポジトリ
            var onegaiRepository = OnegaiRepository.GetRepository(ContextMap.DefaultMap);
            var eventRepository = EventRepository.GetRepository(ContextMap.DefaultMap);
            var monoInfoRepository = new MonoInfoRepository(ContextMap.DefaultMap);
            var mousePurchaceTableRepository = new MousePurchaceTableRepository(ContextMap.DefaultMap);
            var playerOnegaiRepository = PlayerOnegaiRepository.GetRepository(ContextMap.DefaultMap, PlayerContextMap.DefaultMap);
            var playerEventRepository = PlayerEventRepository.GetRepository(ContextMap.DefaultMap, PlayerContextMap.DefaultMap);
            var playerMonoInfoRepository = PlayerMonoInfoRepository.GetRepository(ContextMap.DefaultMap, PlayerContextMap.DefaultMap);
            var playerMouseStockRepository = new PlayerMouseStockRepository(PlayerContextMap.DefaultMap);
            var playerMonoViewRepository = new PlayerMonoViewRepository(monoInfoRepository, PlayerContextMap.DefaultMap);
            var playerArrangementTargetRepository = new PlayerArrangementTargetRepository(monoInfoRepository, playerMonoViewRepository, PlayerContextMap.DefaultMap);
            var playerMouseViewRepository = new PlayerMouseViewRepository(playerArrangementTargetRepository, PlayerContextMap.DefaultMap);
            var playerInfoRepository = new PlayerInfoRepository(PlayerContextMap.DefaultMap);

            // ゲームのコンテキストマップ
            GameContextMap.Initialize(playerArrangementTargetRepository);

            // instance
            this.wallet = new Wallet (new Currency (0),playerInfoRepository); // 所持金の初期値も外出ししたい
            this.arrangementItemStore = new ArrangementItemStore (new ArrangementItemAmount (0), playerInfoRepository); // 所持アイテムの初期値も外出ししたい
            this.arrangementManager = new ArrangementManager (this.rootObject, playerArrangementTargetRepository);
            this.monoManager = new MonoManager (this.rootObject, playerMonoViewRepository);
            this.effectManager = new EffectManager (mainCamera, rootEffectUI);
            this.gameModeManager = new GameModeManager ();
            this.gameModeManager.EnqueueChangeModeWithHistory (GameModeGenerator.GenerateSelectMode ());
            this.fieldRaycastManager = new FieldRaycastManager (this.mainCamera);
            //            this.mouseSelectManager = new MouseSelectManager();
            this.monoSelectManager = new MonoSelectManager ();
            this.timeManager = new TimeManager (playerInfoRepository);
            this.mouseHomeManager = new MouseHomeManager (this.rootObject);
            this.onegaiHomeManager = new OnegaiHomeManager (this.rootObject);
            this.mouseStockManager = new MouseStockManager (this.rootObject, playerMouseStockRepository, playerMouseViewRepository);
            this.dailyActionManager = new DailyActionManager ();
            this.eventManager = new EventManager(playerEventRepository);
            this.constantlyEventPusher = new ConstantlyEventPusher(playerOnegaiRepository);
            this.appearCharacterManager = new AppearCharacterManager(this.rootObject);
            this.dailyAppearCharacterRegistManager = new DailyAppearCharacterRegistManager();
            this.onegaiMediaterManager = new OnegaiMediaterManager(playerOnegaiRepository);
            this.onegaiManager = new OnegaiManager(playerOnegaiRepository);
            this.monoReleaseManager = new MonoReleaseManager(playerMonoInfoRepository);
            this.reserveAmountManager = new ReserveAmountManager();

            // initialize
            this.arrangementPresenter.Initialize(playerArrangementTargetRepository);
            this.gameUIManager.Initialize (onegaiRepository, playerOnegaiRepository,monoInfoRepository, playerMonoInfoRepository, mousePurchaceTableRepository, playerMouseStockRepository);
            this.mouseHomeManager.Initialize ();
            this.onegaiHomeManager.Initialize ();

            // Service
            var initializePlayerInfoService = new InitializePlayerInfoService(playerInfoRepository);
            initializePlayerInfoService.Execute();
            var onegaiUnLockService = new OnegaiUnLockService(onegaiRepository, playerOnegaiRepository);
            onegaiUnLockService.Execute();
            var onegaiUnLockChacheService = new OnegaiUnLockChacheService(playerOnegaiRepository);
            onegaiUnLockChacheService.Execute();
            var eventUnLockService = new EventUnLockService(eventRepository, playerEventRepository);
            eventUnLockService.Execute();
            var initialArrangementService = new InitialArrangementService(playerArrangementTargetRepository);
            initialArrangementService.Execute();
            var initializeOrderedMouseService = new InitializeOrderedMouseService(playerMouseViewRepository);
            initializeOrderedMouseService.Execute();
        }

        private void Update () {
            // イベント関連
            this.constantlyEventPusher.PushConstantlyEventParameter();
            this.eventManager.UpdateByFrame ();

            // 定常
            this.arrangementManager.UpdateByFrame ();
            this.dailyActionManager.UpdateByFrame ();
            this.gameModeManager.UpdateByFrame ();
            this.monoManager.UpdateByFrame ();
            this.fieldRaycastManager.UpdateByFrame ();
            this.timeManager.UpdateByFrame ();
            this.appearCharacterManager.UpdateByFrame();
            this.onegaiManager.UpdateByFrame();
            this.monoReleaseManager.UpdateByFrame();

            // UI関連
            this.gameUIManager.UpdateByFrame();
        }
    }
}