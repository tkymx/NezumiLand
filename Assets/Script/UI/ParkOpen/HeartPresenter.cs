using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class HeartPresenter : UiWindowPresenterBase
    {
        [SerializeField]
        private HeartView heartView = null;

        public void Initialize() {
            this.heartView.Initialize();
            this.Close();
        }

        public void UpdateHeart(int currentHeartCount, int maxHeartCount, int goalHeartCount) {
            this.heartView.UpdateView(currentHeartCount, maxHeartCount, goalHeartCount);
        }
    }    
}