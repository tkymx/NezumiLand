using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NL
{
    public class ParkOpenPromotionView : MonoBehaviour
    {
        [SerializeField]
        private Text promotionCountText = null;

        public void UpdateView(string promotionCount) {
            this.promotionCountText.text = promotionCount;
        }
    }   
}