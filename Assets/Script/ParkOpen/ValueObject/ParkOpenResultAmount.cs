using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {

    public struct ParkOpenResultAmount {

        private int currentHeartCount;
        public int CurrentHeartCount => currentHeartCount;

        private int goalHeartCount;
        public int GoalHeartCount => goalHeartCount;

        public bool IsSuccess {
            get {
                Debug.Assert(this.goalHeartCount > 0, "ゴールが設定されていません。" );
                return this.currentHeartCount >= this.goalHeartCount;
            }
        }

        public ParkOpenResultAmount (int currentHeartCount, int goalHeartCount) {
            this.currentHeartCount = currentHeartCount;
            this.goalHeartCount = goalHeartCount;
        }
    }
}