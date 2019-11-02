using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class GameModeManager
    {
        private IGameMode currentGameMode = null;
        private Queue<IGameMode> nextGameModeQueue = null;
        private Queue<IGameMode> nextGameModeWithHistoryQueue = null;
        private Stack<IGameMode> gameModeHistory = null;

        public GameModeManager()
        {
            this.nextGameModeQueue = new Queue<IGameMode>();
            this.nextGameModeWithHistoryQueue = new Queue<IGameMode>();
            this.gameModeHistory = new Stack<IGameMode>();
        }

        public void EnqueueChangeModeWithHistory(IGameMode gameMode)
        {
            nextGameModeWithHistoryQueue.Enqueue(gameMode);
        }

        private void ChangeModeWithHistory(IGameMode gameMode)
        {
            if (this.currentGameMode != null)
            {
                this.gameModeHistory.Push(this.currentGameMode);
            }
            ChangeMode(gameMode);
        }

        public void EnqueueChangeMode(IGameMode gameMode)
        {
            nextGameModeQueue.Enqueue(gameMode);
        }

        private void ChangeMode(IGameMode gameMode)
        {
            Debug.Assert(gameMode != null, "GameMode が nullです");

            if (this.currentGameMode != null)
            {
                this.currentGameMode.OnExit();
            }
            this.currentGameMode = gameMode;
            this.currentGameMode.OnEnter();
        }

        public void Back()
        {
            Debug.Assert(gameModeHistory.Count > 0, "履歴がありません");
            var gameMode = gameModeHistory.Pop();
            if (gameMode != null)
            {
                EnqueueChangeMode(gameMode);
            }
        }

        public void UpdateByFrame()
        {
            // 履歴を考慮してモードを変更する
            if (nextGameModeWithHistoryQueue.Count > 0)
            {
                var nextGameModeWithHistory = nextGameModeWithHistoryQueue.Dequeue();
                ChangeModeWithHistory(nextGameModeWithHistory);
            }

            // 履歴を考慮せずにモードを変更する
            if (nextGameModeQueue.Count > 0)
            {
                var nextGameMode = nextGameModeQueue.Dequeue();
                ChangeMode(nextGameMode);
            }

            if (this.currentGameMode == null)
            {
                return;
            }
            this.currentGameMode.OnUpdate();
        }

        public override string ToString()
        {
            if (this.currentGameMode == null)
            {
                return "null";
            }
            return this.currentGameMode.ToString();
        }
    }
}
