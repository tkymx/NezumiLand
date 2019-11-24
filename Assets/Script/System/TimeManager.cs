using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    /// <summary>
    /// 経過時間を管理する。ポーズなどを考慮
    /// </summary>
    public class TimeManager {
        private float elapsedTime;
        public float ElapsedTime => elapsedTime;

        private bool isPause;

        public TimeManager () {
            this.elapsedTime = 0;
            this.isPause = false;
        }

        public void Pause () {
            this.isPause = true;
        }

        public void Play () {
            this.isPause = false;
        }

        public float DeltaTime () {
            if (this.isPause) {
                return 0;
            }
            return Time.deltaTime;
        }

        public void UpdateByFrame () {
            this.elapsedTime += DeltaTime ();
        }
    }
}