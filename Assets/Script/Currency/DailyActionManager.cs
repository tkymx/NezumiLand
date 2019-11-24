using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public class DailyActionManager {
        private readonly DailyEarnCalculater dailyEarnCalculater;

        private float prevEarnTime = 0;
        public float PrevEarnTime => prevEarnTime;

        private float daySecond = 10;
        public float DaySecond => daySecond;

        /// <summary>
        /// 一日の残り時間
        /// </summary>
        public float RemainOverSecond {
            get {
                var currentElapsedTime = GameManager.Instance.TimeManager.ElapsedTime;
                return currentElapsedTime - this.prevEarnTime;
            }
        }

        public DailyActionManager (IPlayerOnegaiRepository playerOnegaiRepository) {
            this.dailyEarnCalculater = new DailyEarnCalculater (playerOnegaiRepository);
        }

        public void UpdateByFrame () {
            if (this.IsOverDay ()) {
                var dailyEarn = dailyEarnCalculater.CalcEarnFromSatisfaction ();
                GameManager.Instance.Wallet.Push (dailyEarn);
                GameManager.Instance.EffectManager.PlayEarnEffect (dailyEarn, GameManager.Instance.MouseHomeManager.HomePostion);

                this.OverDay ();
            }
        }

        private bool IsOverDay () {
            var currentElapsedTime = GameManager.Instance.TimeManager.ElapsedTime;
            var elapsedTimeFromPrevEarnTime = currentElapsedTime - this.prevEarnTime;
            return elapsedTimeFromPrevEarnTime > this.daySecond;
        }

        private void OverDay () {
            this.prevEarnTime = GameManager.Instance.TimeManager.ElapsedTime;
        }
    }
}