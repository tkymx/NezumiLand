using System;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    /// <summary>
    /// すでに配置されている ArrangementTarget を移動するために使用します。
    /// moveIndicator が移動先の可視化に使用しています。
    /// movableTarget が移動の対象です。
    /// </summary>
    public class ArrangementMoveIndicatorManager
    {
        private readonly SetArrangementPositionsService setArrangementPositionsService = null;
        private readonly ArrangementTargetCreateService arrangementTargetCreateService = null;

        /// <summary>
        /// 移動先の可視化用
        /// </summary>
        private IPlayerArrangementTarget moveIndicator = null;

        /// <summary>
        /// 移動対象
        /// </summary>
        private IPlayerArrangementTarget movableTarget = null;

        public ArrangementMoveIndicatorManager(IPlayerArrangementTargetRepository playerArrangementTargetRepository)
        {
            this.setArrangementPositionsService = new SetArrangementPositionsService(playerArrangementTargetRepository);
            this.arrangementTargetCreateService = new ArrangementTargetCreateService(playerArrangementTargetRepository);
        }

        /// <summary>
        /// 移動可視化を開始する
        /// </summary>
        public void Start(IPlayerArrangementTarget moveTargetArrangementTarget)
        {
            // 二日目の起動はできない
            if(this.moveIndicator != null || this.movableTarget != null)
            {
                return;
            }

            this.movableTarget = moveTargetArrangementTarget;
            this.moveIndicator = new YesPlayerArrangementTarget(
                movableTarget.CenterPosition, 
                movableTarget.Range, 
                movableTarget.ArrangementPositions, 
                ArrangementTargetState.MoveIndicator,
                movableTarget.MonoInfo,
                ArrangementLayer.MoveIndicator );
                
            GameManager.Instance.ArrangementManager.AddArrangement(this.moveIndicator);
            GameManager.Instance.MonoSelectManager.SelectMonoInfo(this.movableTarget.MonoInfo);
        }

        public void MoveIndicatorPosition(List<ArrangementPosition> positions)
        {
            this.Check();

            this.setArrangementPositionsService.Execute(this.moveIndicator, positions);
        }

        public bool MoveIndicatorPosition(ArrangementManager.Direction direction)
        {
            this.Check();

            // 移動対象がその位置に移動できるかを見る
            var nextPositions = GameManager.Instance.ArrangementManager.GetNearPosition(this.moveIndicator, direction);
            if (!GameManager.Instance.ArrangementManager.IsSetArrangement(nextPositions, this.movableTarget.ArrangementLayer, new List<IPlayerArrangementTarget>(){this.movableTarget}))
            {
                return false;
            }

            // 移動できる場合は移動する
            GameManager.Instance.ArrangementManager.SetPosition(this.moveIndicator, nextPositions);

            return true;
        }

        public void Decision()
        {
            this.Check();

            GameManager.Instance.ArrangementManager.SetPosition(this.movableTarget, this.moveIndicator.ArrangementPositions);
        }

        public void End()
        {
            if (this.moveIndicator != null)
            {
                GameManager.Instance.ArrangementManager.RemoveArranement(this.moveIndicator);
            }            
            GameManager.Instance.MonoSelectManager.RemoveSelect();
            this.moveIndicator = null;
            this.movableTarget = null;
        }

        /// <summary>
        /// 実行時のチェックを行う
        /// </summary>
        private void Check()
        {
            Debug.Assert(this.moveIndicator != null && this.movableTarget != null, "動作対象が設定されていません。");
        }
    }
}