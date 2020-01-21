using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {

    /// <summary>
    /// いきなり出現するキャラクタのモデル
    /// - モデル
    /// </summary>
    public class AppearCharacterModel : ModelBase {
        public string Name { get; private set; }

        public AppearCharacterModel (
            uint id,
            string name) 
        {
            this.Id = id;
            this.Name = name;
        }
    }
}