using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class ScheduleModel : ModelBase
    {
        public float closeElapsedTime { get; private set; }

        public ScheduleModel (
            uint id,
            float closeElapsedTime
        ) {
            this.Id = id;
            this.closeElapsedTime = closeElapsedTime;
        }
    }
}