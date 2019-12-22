using System;
using System.Collections.Generic;
using UnityEngine;

namespace NL {

    public enum ArrangementTargetState {
        None,
        Reserve,
        Appear
    }

    /// <summary>
    /// 配置のターゲット
    /// 配置には２つの座標系がある
    /// - １つ目はプレイヤーが移動するときの目指すべき座標と範囲
    /// - ２つ目はマップの配置が可能かどうかの範囲
    /// </summary>
    public interface IArrangementTarget {
        // 配置の中心
        Vector3 CenterPosition { get; }

        // 配置中心から外径
        float Range { get; }

        // 配置位置
        List<ArrangementPosition> ArrangementPositions { get; }

        // 今後配置されるもの
        MonoInfo MonoInfo { get; set; }

        // 今後設置されているかどうか？
        bool HasMonoInfo { get; }

        // 設定されているモノ
        MonoViewModel MonoViewModel { get; set; }

        // 設置されているかどうか？
        bool HasMonoViewModel { get; }

        // エッジを取得
        List<ArrangementPosition> GetEdgePositions ();

        // 現在の状態
        ArrangementTargetState ArrangementTargetState { get; }

        void ToAppear();
    }
}