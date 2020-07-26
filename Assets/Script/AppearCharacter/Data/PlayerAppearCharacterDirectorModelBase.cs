using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    /// <summary>
    /// プレイヤーキャラクタの動き情報ベース
    /// </summary>
    public abstract class PlayerAppearCharacterDirectorModelBase : ModelBase
    {
        /// <summary>
        /// ディレクターを取得するクラス
        /// ディレクターは派生クラスごとに異なるため抽象化した
        /// </summary>
        /// <returns>ディレクター</returns>
        public abstract IAppearCharacterLifeDirector GetLifeDirectorFromViewModel(AppearCharacterViewModel appearCharacterViewModel);
    }
}

