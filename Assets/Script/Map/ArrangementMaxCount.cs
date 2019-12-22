using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace NL {
    public class ArrangementMaxCount {
        private List<KeyValuePair<uint,long>> monoInfoToCount;

        public List<uint> GetMonoInfoIds () {
            return monoInfoToCount.Select(keyValue => keyValue.Key).Distinct().ToList();
        }

        public long GetMaxCount(uint monoId) {
            var foundIndex = monoInfoToCount.FindIndex(keyValue => keyValue.Key == monoId);
            if (foundIndex < 0) {
                return 0;
            }
            return monoInfoToCount[foundIndex].Value;
        }

        public bool IsValid (ArrangementCount count) {
            foreach (var monoId in count.GetCountedMonoInfos())
            {
                var maxCount = this.GetMaxCount(monoId);
                var currentCount = count.GetCount(monoId);
                Debug.Assert(maxCount != 0 ,monoId.ToString() + "のモノの最大数が０です");
                if (maxCount < currentCount) {
                    return false;
                }
            }
            return true;
        }

        public ArrangementMaxCount () {
            this.monoInfoToCount = new List<KeyValuePair<uint, long>>();
        }

        public ArrangementMaxCount (uint monoInfoId, long count) {
            this.monoInfoToCount = new List<KeyValuePair<uint, long>>();
            this.monoInfoToCount.Add(new KeyValuePair<uint, long>(monoInfoId, count));
        }

        public static ArrangementMaxCount operator + (ArrangementMaxCount left, ArrangementMaxCount right) {
            var arrangementCount = new ArrangementMaxCount ();
            arrangementCount.monoInfoToCount.AddRange(left.monoInfoToCount);
            arrangementCount.monoInfoToCount.AddRange(right.monoInfoToCount);
            return arrangementCount;
        }

        public static ArrangementMaxCount operator - (ArrangementMaxCount left, ArrangementMaxCount right) {
            var arrangementCount = left;
            right.monoInfoToCount.ForEach(keyValue => {
                arrangementCount.monoInfoToCount.Remove(keyValue);
            });
            return arrangementCount;
        }

        public override string ToString () {
            return monoInfoToCount.ToString ();
        }
    }
}