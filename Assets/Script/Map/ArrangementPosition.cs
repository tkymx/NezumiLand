using System;
using UnityEngine;
using System.Collections.Generic;

namespace NL
{
    /// <summary>
    /// 配置位置
    /// </summary>
    public struct ArrangementPosition
    {
        public int x;
        public int z;

        public static bool operator ==(ArrangementPosition left, ArrangementPosition right) {
            return left.x == right.x && left.z == right.z;
        }
        public static bool operator !=(ArrangementPosition left, ArrangementPosition right) {
            return left.x != right.x || left.z != right.z;
        }

        public override bool Equals(object obj)
        {
            var arrangementPosition = (ArrangementPosition)obj;
            return x == arrangementPosition.x && z == arrangementPosition.z;
        }

        public override int GetHashCode() 
        {
            // 実際は32 あるが仕方ない
            return x << 16 & z;
        }
    }


    /// <summary>
    /// 配置位置の差分
    /// </summary>
    public struct ArrangementDiff
    {
        public int dx;
        public int dz;
    }    
}