using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class ParkOpenPromotionPresenter : UiWindowPresenterBase
    {
        [SerializeField]
        private ParkOpenPromotionView parkOpenPromotionView = null;

        public void SetAllPromotionCount(int allPromotionCount) {
            this.parkOpenPromotionView.UpdateView(allPromotionCount.ToString());           
        }
    }    
}