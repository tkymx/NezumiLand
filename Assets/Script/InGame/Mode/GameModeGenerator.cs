using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace NL {
    public class GameModeGenerator {
        public static ArrangementMode GenerateArrangementMode () {
            
            // コンテキストがある前提なのだとしたら、それでも。。。
            var menuSelectModeContext = GameContextMap.DefaultMap.MenuSelectModeContext;
            var arrangementModeContext = GameContextMap.DefaultMap.ArrangementModeContext;

            // コンバート
            arrangementModeContext.SetTagetMonoInfo (menuSelectModeContext.SelectedMonoInfo);

            return new ArrangementMode (GameManager.Instance.MainCamera, GameContextMap.DefaultMap.ArrangementModeContext);
        }
        public static MenuSelectMode GenerateMenuSelectMode () {
            var menuSelectModeContext = GameContextMap.DefaultMap.MenuSelectModeContext;
            return new MenuSelectMode (menuSelectModeContext);
        }
        public static ArrangementMenuSelectMode GenerateArrangementMenuSelectMode (IArrangementTarget arrangementTarget) {
            var arrangementMenuSelectModeContext = GameContextMap.DefaultMap.ArrangementMenuSelectModeContext;

            // コンバート
            arrangementMenuSelectModeContext.SetArrangementTarget (arrangementTarget);

            return new ArrangementMenuSelectMode (arrangementMenuSelectModeContext);
        }

        public static EventMode GenerateEventMode () {
            return new EventMode();
        }

        public static ConversationMode GenerateConversationMode (ConversationModel conversationModel, Action conversationEndCallBack = null) {
            if ( conversationEndCallBack == null ) {
                conversationEndCallBack = () => {};
            }
            return new ConversationMode(conversationModel, conversationEndCallBack);
        }

        public static SelectMode GenerateSelectMode () {
            return new SelectMode ();
        }
    }
}