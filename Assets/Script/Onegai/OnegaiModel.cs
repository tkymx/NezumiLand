using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public enum OnegaiCondition {
        None,
        Near,
        AboveSatisfaction,
        ArrangementCount,
    }

    public enum OnegaiType {
        Unknown,
        Main,
        Sub,
        Person
    }

    public class OnegaiModel : ModelBase {
        public string Title { get; private set; }
        public string Detail { get; private set; }
        public string Author { get; private set; }
        public OnegaiType Type { get; private set; }
        public OnegaiCondition OnegaiCondition { get; private set; }
        public OnegaiConditionArg OnegaiConditionArg { get; private set; }
        public Satisfaction Satisfaction { get; private set; }
        public bool IsInitialLock { get; private set; }
        public ScheduleModel ScheduleModel { get; private set; }

        public bool HasSchedule () {
            return ScheduleModel != null;
        }

        public float CloseTime () {
            if (!HasSchedule()) {
                return float.MaxValue;
            }
            return ScheduleModel.closeElapsedTime;
        }

        public OnegaiModel (
            uint id,
            string title,
            string detail,
            string Author,
            string onegaiType,
            string onegaiCondition,
            string onegaiConditionArg,
            long satisfaction,
            bool isInitialLock,
            ScheduleModel scheduleModel
            ) {
            this.Id = id;
            this.Title = title;
            this.Detail = detail;
            this.Author = Author;

            this.Type = OnegaiType.Unknown;
            if (Enum.TryParse (onegaiType, out OnegaiType outOnegaiType)) {
                this.Type = outOnegaiType;
            }

            this.OnegaiCondition = OnegaiCondition.None;
            if (Enum.TryParse (onegaiCondition, out OnegaiCondition outOnegaiCondition)) {
                this.OnegaiCondition = outOnegaiCondition;
            }

            this.OnegaiConditionArg = new OnegaiConditionArg (onegaiConditionArg);
            this.Satisfaction = new Satisfaction (satisfaction);
            this.IsInitialLock = isInitialLock;
            this.ScheduleModel = scheduleModel;
        }
    }
}