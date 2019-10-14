using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class GameManager : MonoBehaviour
    {
        public GameObject sampleTarget;
        public GameObject makingPrefab;
        public GameObject monoPrefab;
        public Mouse mouse;

        public void Update()
        {
            // テスト用。キャラに移動を養成する
            if(Input.GetKeyDown(KeyCode.Space))
            {
                mouse.OrderMaking(sampleTarget, new PreMono(mouse, makingPrefab, monoPrefab));
            }
        }
    }
}