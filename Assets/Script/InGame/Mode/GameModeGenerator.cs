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
        public static ArrangementMenuSelectMode GenerateArrangementMenuSelectMode (IPlayerArrangementTarget arrangementTarget) {
            var arrangementMenuSelectModeContext = GameContextMap.DefaultMap.ArrangementMenuSelectModeContext;

            // コンバート
            arrangementMenuSelectModeContext.SetArrangementTarget (arrangementTarget);

            return new ArrangementMenuSelectMode (arrangementMenuSelectModeContext);
        }

        public static EventMode GenerateEventMode () {
            return new EventMode();
        }

        public static ConversationMode GenerateConversationMode (ConversationModel conversationModel) {
            return new ConversationMode(conversationModel);
        }

        public static ReceiveRewardMode GenerateReceiveRewardMode (RewardModel rewardModel) {
            return new ReceiveRewardMode(rewardModel);
        }        

        public static SelectMode GenerateSelectMode () {
            return new SelectMode ();
        }

        public static OnegaiSelectMode GenerateOnegaiSelectMode () {
            return new OnegaiSelectMode ();
        }

        public static MousePurchaseMode GenerateMousePurchaseMode () {
            return new MousePurchaseMode ();
        }

        public static ParkOpenMode GenerateParkOpenMode () {
            return new ParkOpenMode();
        }

        public static ParkOpenGroupSelectMode GenerateParkOpenGroupSelectMode () {
            return new ParkOpenGroupSelectMode();
        }        

        public static RemoveArrangementMode GenerateRemoveArrangementMode() {
            return new RemoveArrangementMode();
        }
    }
}