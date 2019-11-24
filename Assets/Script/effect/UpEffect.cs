using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public class UpEffect : MonoBehaviour {
        [SerializeField]
        private float UpSpeed = 1;

        [SerializeField]
        private float UpTime = 1;

        private float elapsedTime = 0;

        private void Start () {
            this.elapsedTime = 0;
        }

        private void Update () {
            // 座標
            transform.position = transform.position + new Vector3 (0, UpSpeed * Time.deltaTime, 0);

            // 時間
            this.elapsedTime += Time.deltaTime;
            if (this.elapsedTime > UpTime) {
                Object.DisAppear (gameObject);
            }
        }
    }
}