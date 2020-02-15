using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    /// <summary>
    /// 遊園地公開時のキャラクタが出現する位置
    /// </summary>
    public class ParkOpenGroupsModel : ModelBase
    {
        public IEnumerable<ParkOpenGroupModel> ParkOpenGroupModels { get; private set; }

        public ParkOpenGroupsModel(
            uint id,
            IEnumerable<ParkOpenGroupModel> parkOpenGroupModels
        )
        {
            this.Id = id;
            this.ParkOpenGroupModels = parkOpenGroupModels;
        }
    }
}

