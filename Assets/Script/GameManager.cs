using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class GameManager : SingletonMonoBehaviour<GameManager>
    {
        public Camera mainCamera;
        public GameObject rootObject;

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
        public ArrangementManager ArrangementManager
        {
            get
            {
                return arrangementManager;
            }
        }

        private TestuserInputPresenter testInputManager;

        private void Start()
        {
            // instance
            this.testInputManager = new TestuserInputPresenter(this.mainCamera);
            this.arrangementManager = new ArrangementManager(this.rootObject);

            // initialize
            this.arrangementUIPresenter.Close();
        }

        private void Update()
        {
            this.testInputManager.UpdateByFrame();
        }
    }
}