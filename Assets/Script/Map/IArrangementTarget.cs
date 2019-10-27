using System;
using UnityEngine;
using System.Collections.Generic;

namespace NL
{
    /// <summary>
    /// 配置のターゲット
    /// 配置には２つの座標系がある
    /// - １つ目はプレイヤーが移動するときの目指すべき座標と範囲
    /// - ２つ目はマップの配置が可能かどうかの範囲
    /// </summary>
    public interface IArrangementTarget
    {
        // プレイヤーが移動に使用する
        Vector3 CenterPosition { get; }
        float Range { get; }

        // 位置情報として使用する
        List<ArrangementPosition> ArrangementPositions { get; }

        // 設定されているモノ
        GameObject Mono { get; set; }
        bool HasMono { get; }
    }
}