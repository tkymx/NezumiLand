using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class GameModeGenerator
    {
        public static ArrangementMode GenerateArrangementMode(Camera mainCamera)
        {
            return new ArrangementMode(mainCamera);
        }
        public static MenuSelectMode GenerateMenuSelectMode()
        {
            return new MenuSelectMode();
        }
    }
}
