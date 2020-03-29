using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

namespace NL {
    public class YesPlayerArrangementTarget : IPlayerArrangementTarget {
        protected Vector3 centerPosition;
        protected float range;
        protected List<ArrangementPosition> arrangementPositions;
        protected ArrangementTargetState arrangementTargetState;
        protected ArrangementLayer arrangementLayer;

        public YesPlayerArrangementTarget(Vector3 centerPosition, float range, List<ArrangementPosition> arrangementPositions, ArrangementTargetState arrangementTargetState, MonoInfo monoInfo, ArrangementLayer arrangementLayer)
        {
            this.centerPosition = centerPosition;
            this.range = range;
            this.arrangementPositions = arrangementPositions;
            this.arrangementTargetState = arrangementTargetState;
            this.MonoInfo = monoInfo;
            this.arrangementLayer = arrangementLayer;
        }

        public YesPlayerArrangementTarget (List<GameObject> gameObjectList, List<ArrangementPosition> arrangementPositions, ArrangementInfo arrangementInfo) {
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

            // はじめは予約
            this.arrangementTargetState = ArrangementTargetState.Reserve;

            // レイヤー
            this.arrangementLayer = ArrangementLayer.Main;
        }

        public PlayerArrangementTargetModel PlayerArrangementTargetModel {
            get {
                return null;
            }
        }

        // プレイヤー
        public Vector3 CenterPosition => centerPosition;
        public float Range => range;

        // 位置情報
        public List<ArrangementPosition> ArrangementPositions => arrangementPositions;

        public virtual void SetPosition(List<ArrangementPosition> positions)
        {
            // 中心座標
            this.centerPosition = new Vector3 ();
            foreach (var position in positions) {
                this.centerPosition += new Vector3(position.x * ArrangementAnnotater.ArrangementWidth, 0 , position.z * ArrangementAnnotater.ArrangementHeight);
            }
            this.centerPosition = centerPosition / positions.Count ();

            // 配列位置
            this.arrangementPositions = positions;         
        }

        // モノ
        public MonoViewModel MonoViewModel { get; private set; }
        public bool HasMonoViewModel => MonoViewModel != null;
        public void RegisterMade (MonoViewModel monoViewModel) {
            this.MonoViewModel = monoViewModel;
        }

        // 配置されるモノ
        public MonoInfo MonoInfo { get; private set; }
        public bool HasMonoInfo => MonoInfo != null;
        public void RegisterMaking (MonoInfo monoInfo) {
            this.MonoInfo = monoInfo;
        }

        // エッジを取得
        public List<ArrangementPosition> GetEdgePositions () {
            throw new NotImplementedException();
        }
        
        // 現在の状態を保持
        public ArrangementTargetState ArrangementTargetState => arrangementTargetState;

        // レイヤー
        public ArrangementLayer ArrangementLayer => this.arrangementLayer;

        // 表示状態にする
        public void ToAppear() {
            Debug.Assert(this.arrangementTargetState != ArrangementTargetState.Appear, "すでにAppearです");
            this.arrangementTargetState = ArrangementTargetState.Appear;
        }
    }
}