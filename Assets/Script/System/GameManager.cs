using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class GameManager : SingletonMonoBehaviour<GameManager>
    {
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

        private MouseStockManager mouseStockManager;
        public MouseStockManager MouseStockManager => mouseStockManager;

        private void Start()
        {
            // コンテキストマップ
            ContextMap.Initialize();
            GameContextMap.Initialize();

            // instance
            this.arrangementManager = new ArrangementManager(this.rootObject);
            this.monoManager = new MonoManager(this.rootObject);
            this.wallet = new Wallet(new Currency(100));
            this.effectManager = new EffectManager(mainCamera, rootEffectUI);
            this.gameModeManager = new GameModeManager();
            this.gameModeManager.EnqueueChangeModeWithHistory(GameModeGenerator.GenerateSelectMode());
            this.fieldRaycastManager = new FieldRaycastManager(this.mainCamera);
//            this.mouseSelectManager = new MouseSelectManager();
            this.monoSelectManager = new MonoSelectManager();
            this.timeManager = new TimeManager();
            this.mouseHomeManager = new MouseHomeManager(this.rootObject);
            this.mouseStockManager = new MouseStockManager(this.rootObject);

            // initialize
            this.gameUIManager.Initialize();
            this.mouseHomeManager.Initialize();
        }

        private void Update()
        {
            this.gameModeManager.UpdateByFrame();
            this.monoManager.UpdateByFrame();
            this.fieldRaycastManager.UpdateByFrame();
            this.timeManager.UpdateByFrame();
        }
    }
}