using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public enum ParkOpenGroupsType
    {
        None,
        Story,
        Village
    }

    /// <summary>
    /// 遊園地公開時のキャラクタが出現する位置
    /// </summary>
    public class ParkOpenGroupsModel : ModelBase
    {
        public ParkOpenGroupsType Type { get; private set; }
        public string BackgroundSpriteName { get; private set; }
        public IEnumerable<ParkOpenGroupModel> ParkOpenGroupModels { get; private set; }

        public ParkOpenGroupsModel(
            uint id,
            ParkOpenGroupsType type,
            string backgroundSpriteName,
            IEnumerable<ParkOpenGroupModel> parkOpenGroupModels
        )
        {
            this.Id = id;
            this.Type = type;
            this.BackgroundSpriteName = backgroundSpriteName;
            this.ParkOpenGroupModels = parkOpenGroupModels;
        }
    }
}

