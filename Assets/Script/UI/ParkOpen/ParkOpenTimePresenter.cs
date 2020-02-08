using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class ParkOpenTimePresenter : UiWindowPresenterBase
    {
        [SerializeField]
        private ParkOpenTimeView parkOpenTimeView = null;

        public void Initialize() {
            this.Close();
        }

        public void UpdateByFrame()
        {
            if (!IsShow()) {
                return;
            }
            
            var time = GameManager.Instance.ParkOpenManager.CurrentParkOpenTimeAmount;
            this.parkOpenTimeView.UpdateView(time.Format(), time.Rate());
        }
    }    
}