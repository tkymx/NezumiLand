using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    /// <summary>
    /// ゲームのモードによるコンテキストの違いを吸収する予定
    /// </summary>
    public class GameContextMap
    {
        // static

        private static GameContextMap defaultMap;
        public static GameContextMap DefaultMap
        {
            get
            {
                Debug.Assert(defaultMap != null, "DefaultMapが初期枯れておりません。");
                return defaultMap;
            }
        }

        public static void Initialize()
        {
            GameContextMap.defaultMap = new GameContextMap();
        }

        // class

        private MenuSelectModeContext menuSelectModeContext = null;
        public MenuSelectModeContext MenuSelectModeContext => menuSelectModeContext;

        private ArrangementModeContext arrangementModeContext = null;
        public ArrangementModeContext ArrangementModeContext => arrangementModeContext;

        public GameContextMap()
        {
            this.menuSelectModeContext = new MenuSelectModeContext();
            this.arrangementModeContext = new ArrangementModeContext();
        }
    }
}
