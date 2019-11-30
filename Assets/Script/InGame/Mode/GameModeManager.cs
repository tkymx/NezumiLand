using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public class GameModeManager {
        private IGameMode currentGameMode = null;
        public IGameMode CurrentGameMode => currentGameMode;

        private IGameMode nextGameMode = null;
        private IGameMode nextGameModeWithHistory = null;
        private Stack<IGameMode> gameModeHistory = null;

        public GameModeManager () {
            this.gameModeHistory = new Stack<IGameMode> ();
        }

        public void EnqueueChangeModeWithHistory (IGameMode gameMode) {
            nextGameModeWithHistory = gameMode;
        }

        private void ChangeModeWithHistory (IGameMode gameMode) {
            if (this.currentGameMode != null) {
                this.gameModeHistory.Push (this.currentGameMode);
            }
            ChangeMode (gameMode);
        }

        public void EnqueueChangeMode (IGameMode gameMode) {
            nextGameMode = gameMode;
        }

        private void ChangeMode (IGameMode gameMode) {
            Debug.Assert (gameMode != null, "GameMode が nullです");

            if (this.currentGameMode != null) {
                this.currentGameMode.OnExit ();
            }
            this.currentGameMode = gameMode;
            this.currentGameMode.OnEnter ();
        }

        public void Back () {
            Debug.Assert (gameModeHistory.Count > 0, "履歴がありません");
            var gameMode = gameModeHistory.Pop ();
            if (gameMode != null) {
                EnqueueChangeMode (gameMode);
            }
        }

        public void UpdateByFrame () {
            // 履歴を考慮してモードを変更する
            if (this.nextGameModeWithHistory != null) {
                var tempNextGameModeWithHistory = this.nextGameModeWithHistory;
                this.nextGameModeWithHistory = null;
                ChangeModeWithHistory (tempNextGameModeWithHistory);
            }

            // 履歴を考慮せずにモードを変更する
            if (this.nextGameMode != null) {
                var tempNextGameMode = this.nextGameMode;
                this.nextGameMode = null;
                ChangeMode (tempNextGameMode);
            }

            if (this.currentGameMode == null) {
                return;
            }
            this.currentGameMode.OnUpdate ();
        }

        public bool IsEventMode => this.currentGameMode is EventMode;

        public override string ToString () {
            if (this.currentGameMode == null) {
                return "null";
            }
            return this.currentGameMode.ToString ();
        }
    }
}