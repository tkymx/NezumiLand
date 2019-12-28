using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace NL {
    public class DailyActionManager {

        private bool isInitialize = false;

        private int prevDay = 0;

        public DailyActionManager () {
            this.isInitialize = false;
        }

        public void UpdateByFrame () {

            if (!this.isInitialize) {
                var span = DayTextConverter.ConvertSpan(GameManager.Instance.TimeManager.ElapsedTime);
                this.prevDay = span.Days;
                this.isInitialize = true;
            }

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