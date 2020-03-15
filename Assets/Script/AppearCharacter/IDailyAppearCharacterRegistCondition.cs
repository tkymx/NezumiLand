using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public interface IDailyAppearCharacterRegistCondition
    {
        /// <summary>
        /// 予約されたキャラクタを出現させるかどうか？
        /// (例) 確率で出現するキャラクタなどは、確率で true が帰ってくる
        /// </summary>
        bool IsResist();

        /// <summary>
        /// 一度出現したら引っ込んだらもう出現しない
        /// </summary>
        bool IsOnce();
    }
}