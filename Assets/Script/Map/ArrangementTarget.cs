using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NL {
    public class ArrangementTarget : IArrangementTarget {
        Vector3 centerPosition;
        float range;
        List<ArrangementPosition> arrangementPositions;

        public ArrangementTarget (List<GameObject> gameObjectList, List<ArrangementPosition> arrangementPositions, ArrangementInfo arrangementInfo) {
            // 中心座標
            this.centerPosition = new Vector3 ();
            foreach (var gameObject in gameObjectList) {
                this.centerPosition += gameObject.transform.position;
            }
            this.centerPosition = centerPosition / gameObjectList.Count ();

            // 半径
            this.range = (arrangementInfo.mono.Height * ArrangementAnnotater.ArrangementHeight + arrangementInfo.mono.Width * ArrangementAnnotater.ArrangementWidth) / 2 / 2;

            // 配列位置
            this.arrangementPositions = new List<ArrangementPosition> (arrangementPositions);
        }

        // プレイヤー
        public Vector3 CenterPosition => centerPosition;
        public float Range => range;

        // 位置情報
        public List<ArrangementPosition> ArrangementPositions => arrangementPositions;

        // モノ
        public MonoViewModel MonoViewModel { get; set; }
        public bool HasMonoViewModel => MonoViewModel != null;

        // 配置されるモノ
        public MonoInfo MonoInfo { get; set; }
        public bool HasMonoInfo => MonoInfo != null;

        // エッジを取得
        public List<ArrangementPosition> GetEdgePositions () {
            var edgePositions = new List<ArrangementPosition> ();
            var diffs = new List<ArrangementDiff> () {
                new ArrangementDiff () { dx = -1, dz = -1 },
                new ArrangementDiff () { dx = 1, dz = 1 },
                new ArrangementDiff () { dx = -1, dz = 1 },
                new ArrangementDiff () { dx = 1, dz = -1 }
            };

            foreach (var arrangementPosition in arrangementPositions) {
                foreach (var diff in diffs) {
                    var diffPosition = new ArrangementPosition () {
                        x = arrangementPosition.x + diff.dx,
                        z = arrangementPosition.z + diff.dz
                    };

                    var foundOwnIndex = arrangementPositions.FindIndex (position => position == diffPosition);
                    if (foundOwnIndex >= 0) {
                        continue;
                    }

                    var foundEdgeIndex = edgePositions.FindIndex (position => position == diffPosition);
                    if (foundEdgeIndex >= 0) {
                        continue;
                    }

                    edgePositions.Add (diffPosition);
                }
            }
            return edgePositions;
        }
    }
}