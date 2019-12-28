using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    /// <summary>
    /// 経過時間を管理する。ポーズなどを考慮
    /// </summary>
    public class TimeManager {

        private UpdateTimeService updateTimeService = null;

        private float elapsedTime;
        public float ElapsedTime => elapsedTime;

        private bool isPause;
        public bool IsPause => isPause;

        public TimeManager (IPlayerInfoRepository playerInfoRepository) {
            this.elapsedTime = 0;
            this.isPause = false;

            this.updateTimeService = new UpdateTimeService(playerInfoRepository);
        }

        public void ForceSet (float elapsedTime) {
            this.elapsedTime = elapsedTime;
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

        public float DeltaTimeWithoutPause () {
            return Time.deltaTime;
        }

        private float updateTime = 0;
        private const float UpdateTimeInterval = 1.0f;

        public void UpdateByFrame () {
            this.elapsedTime += DeltaTime ();
            
            this.updateTime  += DeltaTime ();
            if (this.updateTime > UpdateTimeInterval) {
                this.updateTimeService.Execute(this.elapsedTime);
                this.updateTime = 0;
            }
        }

        public override string ToString() {
            return DayTextConverter.ConvertString(this.elapsedTime);
        }
    }
}