using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    /// <summary>
    /// ゲームのモードによるコンテキストの違いを吸収する予定
    /// </summary>
    public class GameContextMap {
        // static

        private static GameContextMap defaultMap;
        public static GameContextMap DefaultMap {
            get {
                Debug.Assert (defaultMap != null, "DefaultMapが初期枯れておりません。");
                return defaultMap;
            }
        }

        public static void Initialize (IPlayerArrangementTargetRepository playerArrangementTargetRepository) {
            GameContextMap.defaultMap = new GameContextMap (playerArrangementTargetRepository);
        }

        // class

        private MenuSelectModeContext menuSelectModeContext = null;
        public MenuSelectModeContext MenuSelectModeContext => menuSelectModeContext;

        private ArrangementModeContext arrangementModeContext = null;
        public ArrangementModeContext ArrangementModeContext => arrangementModeContext;

        private ArrangementMenuSelectModeContext arrangementMenuSelectModeContext = null;
        public ArrangementMenuSelectModeContext ArrangementMenuSelectModeContext => arrangementMenuSelectModeContext;

        public GameContextMap (IPlayerArrangementTargetRepository playerArrangementTargetRepository) {
            this.menuSelectModeContext = new MenuSelectModeContext ();
            this.arrangementModeContext = new ArrangementModeContext (playerArrangementTargetRepository);
            this.arrangementMenuSelectModeContext = new ArrangementMenuSelectModeContext ();
        }
    }
}