using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public class OnegaiHomeManager {
        private GameObject root = null;
        private GameObject homeObject = null;

        /// <summary>
        /// 家の位置
        /// </summary>
        public Vector3 HomePostion => homeObject.transform.position;
        public float HomeRange => 1.0f;

        public OnegaiHomeManager (GameObject root) {
            this.root = root;
        }

        public void Initialize () {
            AppearHome ();
        }

        /// <summary>
        /// 家オブジェクトを配置する
        /// </summary>
        private void AppearHome () {
            var homePrefab = ResourceLoader.LoadModel ("onegai_home");
            homeObject = Object.AppearToFloor (homePrefab, root, new Vector3 (10, 0, 5) /*TODO 後で変える*/ );
        }
    }
}