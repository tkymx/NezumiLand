using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace NL
{
    public enum EventState
    {
        Lock,
        UnLock,
        Clear
    }

    public class PlayerEventModel
    {
        public uint Id { get; private set; }
        public EventModel EventModel { get; private set; }
        public EventState EventState { get; private set; }
        public bool[] IsConditionDone { get; private set; }

        public void ToDone(int eventIndex) {
            this.IsConditionDone[eventIndex] = true;

            // すべてがDoneになったらクリア
            if (this.IsConditionDone.All(isDone => isDone == true)) {
                EventState = EventState.Clear;
            }
        }

        public bool HasDetectableCondition(EventConditionType eventConditionType) {
            for (int i = 0; i < this.IsConditionDone.Length; i++)
            {
                if (this.IsConditionDone[i]) {
                    continue;
                }
                if (this.EventModel.EventConditionModels[i].EventConditionType == eventConditionType) {
                    return true;
                }    
            }
            return false;
        }

        public PlayerEventModel(
            uint id,
            EventModel eventModel,
            string eventState,
            bool[] isConditionDone)
        {
            this.Id = id;
            this.EventModel = eventModel;

            this.EventState = EventState.Lock;
            if (Enum.TryParse(eventState, out EventState outEventState))
            {
                this.EventState = outEventState;
            }

            this.IsConditionDone = isConditionDone;
        }
    }
}

