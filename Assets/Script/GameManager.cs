using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class GameManager : MonoBehaviour
    {
        public Camera mainCamera;
        public GameObject sampleTarget;
        public GameObject makingPrefab;
        public GameObject monoPrefab;
        public GameObject rootObject;
        public Mouse mouse;

        private TestuserInputManager testInputManager;

        private void Start()
        {
            testInputManager = new TestuserInputManager(mainCamera, rootObject);
        }

        private void Update()
        {
            // テスト用。キャラに移動を養成する
            if(Input.GetKeyDown(KeyCode.Space))
            {
                mouse.OrderMaking(sampleTarget, new PreMono(mouse, makingPrefab, monoPrefab));
            }

            testInputManager.UpdateByFrame();
        }
    }
}