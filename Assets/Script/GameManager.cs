using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class GameManager : SingletonMonoBehaviour<GameManager>
    {
        public Camera mainCamera;
        public GameObject rootObject;
        public GameObject rootEffectUI;

        [SerializeField]
        private ArrangementPresenter arrangementPresenter = null;
        public ArrangementPresenter ArrangementPresenter => arrangementPresenter;

        [SerializeField]
        private ArrangementUIPresenter arrangementUIPresenter = null;
        public ArrangementUIPresenter ArrangementUIPresenter => arrangementUIPresenter;

        [SerializeField]
        private Mouse mouse = null;
        public Mouse Mouse => mouse;

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

        private void Start()
        {
            // instance
            this.arrangementManager = new ArrangementManager(this.rootObject);
            this.monoManager = new MonoManager(this.rootObject);
            this.wallet = new Wallet(new Currency(100));
            this.effectManager = new EffectManager(mainCamera, rootEffectUI);
            this.gameModeManager = new GameModeManager(GameModeGenerator.GenerateArrangementMode(mainCamera));

            // initialize
            this.arrangementUIPresenter.Close();
        }

        private void Update()
        {
            this.gameModeManager.UpdateByFrame();
            this.monoManager.UpdateByFrame();
        }
    }
}