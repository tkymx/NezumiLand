using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    /// <summary>
    /// 遊園地公開時のキャラクタが出現する位置
    /// </summary>
    public class ParkOpenPositionModel : ModelBase
    {
        public enum PositionType
        {
            None,
            Appear,
            DisAppear
        }

        public Vector3 Position { get; private set; }
        public PositionType Type { get; private set; }

        public ParkOpenPositionModel(
            uint id,
            Vector3 position,
            PositionType type
        )
        {
            this.Id = id;
            this.Position = position;
            this.Type = type;
        }
    }
}

