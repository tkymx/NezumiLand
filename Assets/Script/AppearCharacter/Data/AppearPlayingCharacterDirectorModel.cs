using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    /// <summary>
    /// 移動するキャラクタの動き情報
    /// </summary>
    public class AppearPlayingCharacterDirectorModel : AppearCharacterDirectorModelBase
    {
        public AppearPlayingCharacterDirectorModel(
            uint id,
            AppearCharacterModel appearCharacterModel
        )
        {
            this.Id = id;
            this.AppearCharacterModel = appearCharacterModel;
        }
    }
}