using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    namespace ParkOpenCardAction
    {
        /// <summary>
        /// 開放時のカードの効果の実行インターフェース
        /// </summary>
        public interface IParkOpenCardAction
        {
            ParkOpenCardModel ParkOpenCardModel { get; }
            void OnStart(ParkOpenCardModel parkOpenCardModel);
            void OnUpdate();
            bool IsAlive();
            void OnComplate();
        }
    }
}
