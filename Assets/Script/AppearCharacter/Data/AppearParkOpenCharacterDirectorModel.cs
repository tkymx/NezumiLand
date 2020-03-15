using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    /// <summary>
    /// 公園開放するキャラクタの動き情報
    /// </summary>
    public class AppearParkOpenCharacterDirectorModel : AppearCharacterDirectorModelBase
    {
        public AppearParkOpenCharacterDirectorModel(
            uint id,
            AppearCharacterModel appearCharacterModel
        )
        {
            this.Id = id;
            this.AppearCharacterModel = appearCharacterModel;
        }
    }
}

