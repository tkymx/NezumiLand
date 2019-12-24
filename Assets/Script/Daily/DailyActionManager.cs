using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace NL {
    public class DailyActionManager {

        private int prevDay = 0;

        public DailyActionManager () {
            var span = DayTextConverter.ConvertSpan(GameManager.Instance.TimeManager.ElapsedTime);
            this.prevDay = span.Days;
        }

        public void UpdateByFrame () {
            if (this.IsOverDay ()) {    
                GameManager.Instance.GameModeManager.EnqueueChangeMode(GameModeGenerator.GenerateDailyChangeMode());
                this.OverDay ();
            }
        }

        private bool IsOverDay () {
            var span = DayTextConverter.ConvertSpan(GameManager.Instance.TimeManager.ElapsedTime);
            return span.Days > this.prevDay;
        }

        private void OverDay () {
            var span = DayTextConverter.ConvertSpan(GameManager.Instance.TimeManager.ElapsedTime);
            this.prevDay = span.Days;
        }
    }
}