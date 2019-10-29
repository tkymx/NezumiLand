using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class GameModeManager
    {
        private IGameMode currentGameMode = null;

        public GameModeManager(IGameMode gameMode)
        {
            this.ChangeMode(gameMode);
        }

        public void ChangeMode(IGameMode gameMode)
        {
            Debug.Assert(gameMode != null, "GameMode が nullです");

            if(this.currentGameMode != null)
            {
                this.currentGameMode.OnExit();
            }
            this.currentGameMode = gameMode;
            this.currentGameMode.OnEnter();
        }

        public void UpdateByFrame()
        {
            if (this.currentGameMode == null)
            {
                return;
            }
            this.currentGameMode.OnUpdate();
        }
    }
}
