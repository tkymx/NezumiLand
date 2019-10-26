using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class GameManager : SingletonMonoBehaviour<GameManager>
    {
        public Camera mainCamera;
        public GameObject rootObject;
        public ArrangementPresenter arrangementPresenter;

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
            var mono = new MonoInfo()
            {
                Width = 2,
                Height = 2,
                monoPrefab = ResourceLoader.LoadPrefab("Model/branko"),
            };
            testInputManager = new TestuserInputPresenter(mainCamera, mouse, mono, rootObject);
            arrangementManager = new ArrangementManager(arrangementPresenter);
        }

        private void Update()
        {
            testInputManager.UpdateByFrame();
        }
    }
}