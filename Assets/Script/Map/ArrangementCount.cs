using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace NL {
    public struct ArrangementCount : IConsumableAmount {

        private Dictionary<uint,long> monoInfoToCount_impl;
        private Dictionary<uint,long> monoInfoToCount { 
            get {
                if (monoInfoToCount_impl == null) {
                    monoInfoToCount_impl = new Dictionary<uint, long>();
                }
                return monoInfoToCount_impl;
            }
        }

        public List<uint> GetCountedMonoInfos () {
            return monoInfoToCount.Keys.ToList();
        }

        public long GetCount(uint monoId) {
            if (this.monoInfoToCount.ContainsKey(monoId)) {
                return this.monoInfoToCount[monoId];
            }
            return 0;
        }

        public ArrangementCount (uint monoInfoId) {
            this.monoInfoToCount_impl = new Dictionary<uint, long>();
            this.monoInfoToCount[monoInfoId] = 1;
        }

        public void Add (uint monoId, long count) {
            if (!this.monoInfoToCount.ContainsKey(monoId)) {
                this.monoInfoToCount[monoId] = 0;
            }

            this.monoInfoToCount[monoId] += count;

            if (this.monoInfoToCount[monoId] < 0 ) {
                this.monoInfoToCount[monoId] = 0;
            }
        }

        public IConsumableAmount Add_Implementation(IConsumableAmount right) {
            Debug.Assert(right is ArrangementCount, "ArrangementCount ではありません");
            var rightCast = (ArrangementCount)right;

            var arrangementCount = this;
            foreach (var keyValue in rightCast.monoInfoToCount)
            {
                arrangementCount.Add(keyValue.Key, keyValue.Value);
            }
            return arrangementCount;
        }

        public IConsumableAmount Subtraction_Implementation(IConsumableAmount right) {
            Debug.Assert(right is ArrangementCount, "ArrangementCount ではありません");
            var rightCast = (ArrangementCount)right;

            var arrangementCount = this;
            foreach (var keyValue in rightCast.monoInfoToCount)
            {
                arrangementCount.Add(keyValue.Key, keyValue.Value);
            }
            return arrangementCount;
        }

        public static ArrangementCount operator + (ArrangementCount left, ArrangementCount right) {
            return (ArrangementCount)left.Add_Implementation(right);
        }

        public static ArrangementCount operator - (ArrangementCount left, ArrangementCount right) {
            return (ArrangementCount)left.Subtraction_Implementation(right);
        }

        public override string ToString () {
            return monoInfoToCount.ToString ();
        }

        public static ArrangementCount Zero {
            get {
                return default(ArrangementCount);            
            }
        }
    }
}