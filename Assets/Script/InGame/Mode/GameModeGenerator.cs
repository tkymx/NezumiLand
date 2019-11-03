using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class GameModeGenerator
    {
        public static ArrangementMode GenerateArrangementMode()
        {
            var arrangementModeContext = GameContextMap.DefaultMap.ArrangementModeContext;
            var menuSelectModeContext = GameContextMap.DefaultMap.MenuSelectModeContext;

            // コンバート
            arrangementModeContext.SetTagetMonoInfo(menuSelectModeContext.SelectedMonoInfo);

            return new ArrangementMode(GameManager.Instance.MainCamera, GameContextMap.DefaultMap.ArrangementModeContext);
        }
        public static MenuSelectMode GenerateMenuSelectMode()
        {
            return new MenuSelectMode(GameContextMap.DefaultMap.MenuSelectModeContext);
        }
        public static ArrangementMenuSelectMode GenerateArrangementMenuSelectMode(IArrangementTarget arrangementTarget)
        {
            return new ArrangementMenuSelectMode(arrangementTarget);
        }
        public static SelectMode GenerateSelectMode()
        {
            return new SelectMode();
        }
    }
}
