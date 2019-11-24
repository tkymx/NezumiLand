using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NL {
    public enum EventState {
        Lock,
        UnLock,
        Clear,
        Done
    }

    public class PlayerEventModel {
        public uint Id { get; private set; }
        public EventModel EventModel { get; private set; }
        public EventState EventState { get; private set; }
        public List<EventConditionModel> doneEventConditionModels { get; private set; }

        public void ToClear (EventConditionModel eventConditionModel) {
            if (this.doneEventConditionModels.IndexOf (eventConditionModel) >= 0) {
                return;
            }

            this.doneEventConditionModels.Add (eventConditionModel);

            // すべてがDoneになったらクリア
            if (this.doneEventConditionModels.Count >= EventModel.EventConditionModels.Count) {
                EventState = EventState.Clear;
            }
        }

        public void ToDone () {
            this.EventState = EventState.Done;
        }

        /// <summary>
        /// 検証に使用する条件があるかを検査する
        /// done になっていない条件の中から該当の条件を抜き出す
        /// </summary>
        /// <param name="eventConditionType"></param>
        /// <returns></returns>
        public bool HasDetectableCondition (EventConditionType eventConditionType) {
            foreach (var eventConditionModel in this.EventModel.EventConditionModels) {
                // done に該当のモデルがあれば見ない
                if (this.doneEventConditionModels.IndexOf(eventConditionModel) >= 0) {
                    continue;
                }
                if (eventConditionModel.EventConditionType == eventConditionType) {
                    return true;
                }
            }
            return false;
        }

        public List<EventConditionModel> Yets () {
            return this.EventModel.EventConditionModels.Where (model => this.doneEventConditionModels.IndexOf (model) < 0).ToList ();
        }

        public PlayerEventModel (
            uint id,
            EventModel eventModel,
            string eventState,
            List<EventConditionModel> doneEventConditionModels) {
            this.Id = id;
            this.EventModel = eventModel;

            this.EventState = EventState.Lock;
            if (Enum.TryParse (eventState, out EventState outEventState)) {
                this.EventState = outEventState;
            }

            this.doneEventConditionModels = doneEventConditionModels;
        }
    }
}