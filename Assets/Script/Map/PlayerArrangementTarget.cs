using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NL {
    public class PlayerArrangementTarget : IPlayerArrangementTarget {

        private PlayerArrangementTargetModel playerArrangementTargetModel;
        public PlayerArrangementTargetModel PlayerArrangementTargetModel => playerArrangementTargetModel;

        public PlayerArrangementTarget (PlayerArrangementTargetModel playerArrangementTargetModel) {

            this.playerArrangementTargetModel = playerArrangementTargetModel;
        }

        // プレイヤー
        public Vector3 CenterPosition => playerArrangementTargetModel.CenterPosition;
        public float Range => playerArrangementTargetModel.Range;

        // 位置情報
        public List<ArrangementPosition> ArrangementPositions => playerArrangementTargetModel.Positions;

        public void SetPosition(List<ArrangementPosition> positions)
        {
            this.playerArrangementTargetModel.SetPosition(positions);
        }

        // モノ
        public MonoViewModel MonoViewModel { get; private set; }
        public bool HasMonoViewModel => MonoViewModel != null;
        public void RegisterMade (MonoViewModel monoViewModel) {
            this.MonoViewModel = monoViewModel;
            this.PlayerArrangementTargetModel.SetPlayerMonoView(monoViewModel.PlayerMonoViewModel);
        }

        // 配置されるモノ
        public MonoInfo MonoInfo => this.playerArrangementTargetModel.MonoInfo;
        public bool HasMonoInfo => this.playerArrangementTargetModel.MonoInfo != null;
        public void RegisterMaking (MonoInfo monoInfo) {
            this.playerArrangementTargetModel.SetMonoInfo(monoInfo);
        }

        // エッジを取得
        public List<ArrangementPosition> GetEdgePositions () {
            var edgePositions = new List<ArrangementPosition> ();
            var diffs = new List<ArrangementDiff> () {
                new ArrangementDiff () { dx = 0, dz = 1 },
                new ArrangementDiff () { dx = 0, dz = -1 },
                new ArrangementDiff () { dx = 1, dz = 0 },
                new ArrangementDiff () { dx = -1, dz = 0 }
            };

            foreach (var arrangementPosition in ArrangementPositions) {
                foreach (var diff in diffs) {
                    var diffPosition = new ArrangementPosition () {
                        x = arrangementPosition.x + diff.dx,
                        z = arrangementPosition.z + diff.dz
                    };

                    var foundOwnIndex = ArrangementPositions.FindIndex (position => position == diffPosition);
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
        
        // 現在の状態を保持
        public ArrangementTargetState ArrangementTargetState => playerArrangementTargetModel.State;

        // レイヤー
        public ArrangementLayer ArrangementLayer => ArrangementLayer.Main;

        // 表示状態にする
        public void ToAppear() {
            this.playerArrangementTargetModel.ToAppear();
        }
    }
}