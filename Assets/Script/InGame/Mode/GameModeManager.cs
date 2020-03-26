using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace NL {
    public class GameModeManager {
        private IGameMode currentGameMode = null;
        public IGameMode CurrentGameMode => currentGameMode;

        private IGameMode nextGameMode = null;
        private IGameMode nextGameModeWithHistory = null;
        private Stack<IGameMode> gameModeHistory = null;

        // 特定のモードが終了したときのオブザーバブル
        private Dictionary<IGameMode,TypeObservable<int>> modeEndObservableDictionary = null;

        public TypeObservable<int> GetModeEndObservable(IGameMode gameMode) {
            if (!this.modeEndObservableDictionary.ContainsKey(gameMode)) {
                this.modeEndObservableDictionary.Add(gameMode, new TypeObservable<int>());
            }
            return this.modeEndObservableDictionary[gameMode];
        }

        public GameModeManager () {
            this.gameModeHistory = new Stack<IGameMode> ();
            this.modeEndObservableDictionary = new Dictionary<IGameMode, TypeObservable<int>>(); 
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

                // 終了を流す
                var observable = this.GetModeEndObservable(this.currentGameMode);
                observable.Execute(0);
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

        //TODO おいおいは、モードのはじめと終わりで制御できるようになりたい
        public bool IsCameraMobableMode => 
            this.currentGameMode is SelectMode || 
            this.currentGameMode is ArrangementMode || 
            this.currentGameMode is ArrangementMenuSelectMode ||
            this.currentGameMode is RemoveArrangementMode || 
            this.currentGameMode is ParkOpenMode;
            
        public bool IsEventMode => this.currentGameMode is EventMode;
        public bool IsMousePurchaseMode => this.currentGameMode is MousePurchaseMode;
        public bool IsMenuSelectMode => this.currentGameMode is MenuSelectMode;
        public bool IsArrangementMode => this.currentGameMode is ArrangementMode;

        public override string ToString () {
            if (this.currentGameMode == null) {
                return "null";
            }
            return this.currentGameMode.ToString ();
        }
    }
}