using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class GameManager : MonoBehaviour
    {
        public Camera mainCamera;
        public GameObject rootObject;
        public Mouse mouse;

        private TestuserInputManager testInputManager;

        private void Start()
        {
            var mono = new Mono()
            {
                Width = 2,
                Height = 2,
                monoPrefab = ResourceLoader.LoadPrefab("Model/branko"),
            };
            testInputManager = new TestuserInputManager(mainCamera, mouse, mono, rootObject);
        }

        private void Update()
        {
            testInputManager.UpdateByFrame();
        }
    }
}