using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NL
{
    public class ParkOpenResultObtainRewardInfoCellView : ListCellViewBase
    {
        [SerializeField]
        private Text title = null;

        [SerializeField]
        private Text result = null;

        public void UpdateView(string title, string result) {
            this.title.text = title;
            this.result.text = result;
        }
    }   
}