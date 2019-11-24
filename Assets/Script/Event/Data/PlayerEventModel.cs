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
        public List<EventConditionModel> doneEventConditionModels { get; private set; }

        public void ToDone(EventConditionModel eventConditionModel) {
            if (this.doneEventConditionModels.IndexOf(eventConditionModel) >= 0) {
                return;
            }

            this.doneEventConditionModels.Add(eventConditionModel);

            // すべてがDoneになったらクリア
            if (this.doneEventConditionModels.Count >= EventModel.EventConditionModels.Length) {
                EventState = EventState.Clear;
            }
        }

        public bool HasDetectableCondition(EventConditionType eventConditionType) {
            foreach (var eventConditionMoel in this.doneEventConditionModels)
            {                
                if (eventConditionMoel.EventConditionType == eventConditionType) {
                    return true;
                }
            }
            return false;
        }

        public List<EventConditionModel> Yets() {
            return this.EventModel.EventConditionModels.Where(model => this.doneEventConditionModels.IndexOf(model) < 0).ToList();
        }

        public PlayerEventModel(
            uint id,
            EventModel eventModel,
            string eventState,
            List<EventConditionModel> doneEventConditionModels)
        {
            this.Id = id;
            this.EventModel = eventModel;

            this.EventState = EventState.Lock;
            if (Enum.TryParse(eventState, out EventState outEventState))
            {
                this.EventState = outEventState;
            }

            this.doneEventConditionModels = doneEventConditionModels;
        }
    }
}

