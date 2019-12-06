using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public enum OnegaiState {
        Lock, // 基本的にロックはされない認識、変なのが入らないためのセーフティ
        UnLock,
        Clear
    }

    public class PlayerOnegaiModel {
        public uint Id { get; private set; }
        public OnegaiModel OnegaiModel { get; private set; }
        public OnegaiState OnegaiState { get; private set; }

        public void ToClear () {
            this.OnegaiState = OnegaiState.Clear;
            GameManager.Instance.EventManager.PushEventParameter(new NL.EventCondition.ClearOnegai(OnegaiModel));
        }

        public void ToUnlock () {
            this.OnegaiState = OnegaiState.UnLock;
        }

        public bool IsLock() {
            return this.OnegaiState == OnegaiState.Lock;
        }

        public PlayerOnegaiModel (
            uint id,
            OnegaiModel onegaiModel,
            string onegaiState) {
            this.Id = id;
            this.OnegaiModel = onegaiModel;

            this.OnegaiState = OnegaiState.Lock;
            if (Enum.TryParse (onegaiState, out OnegaiState outOnegaiState)) {
                this.OnegaiState = outOnegaiState;
            }
        }
    }
}