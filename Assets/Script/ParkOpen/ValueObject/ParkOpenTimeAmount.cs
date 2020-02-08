using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {

    public struct ParkOpenTimeAmount {

        private DayTextConverter dayTextConverter;
        private float limitTime;
        private float elapsedTime;
        public float ElapsedTime => elapsedTime;

        public string Format()
        {
            return this.dayTextConverter.ConvertStringPerDay(this.elapsedTime);
        }

        public float Rate()
        {
            return this.elapsedTime / this.limitTime;
        }

        public ParkOpenTimeAmount (float elapsedTime, float limitTime) {
            this.elapsedTime = elapsedTime;
            this.limitTime = limitTime;
            this.dayTextConverter = new DayTextConverter(limitTime);
        }

        public static ParkOpenTimeAmount operator + (ParkOpenTimeAmount left, float right) {
            return new ParkOpenTimeAmount(left.elapsedTime + right, left.limitTime);
        }

        public static ParkOpenTimeAmount operator - (ParkOpenTimeAmount left, float right) {
            return new ParkOpenTimeAmount(left.elapsedTime - right, left.limitTime);
        }

        public static bool operator < (ParkOpenTimeAmount left, ParkOpenTimeAmount right) {
            return left.elapsedTime < right.elapsedTime;
        }

        public static bool operator > (ParkOpenTimeAmount left, ParkOpenTimeAmount right) {
            return left.elapsedTime > right.elapsedTime;
        }

        public static bool operator <= (ParkOpenTimeAmount left, ParkOpenTimeAmount right) {
            return left.elapsedTime <= right.elapsedTime;
        }

        public static bool operator >= (ParkOpenTimeAmount left, ParkOpenTimeAmount right) {
            return left.elapsedTime >= right.elapsedTime;
        }
    }
}