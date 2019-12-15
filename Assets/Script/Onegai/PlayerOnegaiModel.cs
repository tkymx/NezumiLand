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

    public class PlayerOnegaiModel : ModelBase {

        public OnegaiModel OnegaiModel { get; private set; }
        public OnegaiState OnegaiState { get; private set; }
        public float StartOnegaiTime { get; private set; } 

        public void ToClear () {
            this.OnegaiState = OnegaiState.Clear;
            GameManager.Instance.EventManager.PushEventParameter(new NL.EventCondition.ClearOnegai(OnegaiModel));
        }

        public void ToUnlock () {
            this.OnegaiState = OnegaiState.UnLock;
        }

        public void ToLock () {
            this.OnegaiState = OnegaiState.Lock;
        }

        public bool IsLock() {
            return this.OnegaiState == OnegaiState.Lock;
        }

        public float CloseTime () {
            if (!HasSchedule()) {
                return float.MaxValue;
            }
            return this.StartOnegaiTime + this.OnegaiModel.ScheduleModel.closeElapsedTime;
        }

        public bool HasSchedule () {
            return OnegaiModel.HasSchedule();
        }

        public bool IsClose (float currentElapsedTime) {
            if (!HasSchedule ()) {
                return false;
            } 
            var closeTime = CloseTime ();
            var isClose = currentElapsedTime > closeTime;
            if (isClose) {
                return true;
            }
            return false;
        }

        public PlayerOnegaiModel (
            uint id,
            OnegaiModel onegaiModel,
            string onegaiState,
            float startOnegaiTime) {
            this.Id = id;
            this.OnegaiModel = onegaiModel;

            this.OnegaiState = OnegaiState.Lock;
            if (Enum.TryParse (onegaiState, out OnegaiState outOnegaiState)) {
                this.OnegaiState = outOnegaiState;
            }

            this.StartOnegaiTime = startOnegaiTime;
        }
    }
}