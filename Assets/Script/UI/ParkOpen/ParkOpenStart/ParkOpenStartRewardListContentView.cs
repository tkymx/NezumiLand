using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace NL
{
    public class ParkOpenStartRewardListContentView : ParkOpenStartRewardListCellViewBase
    {
        [SerializeField]
        private Text itemName = null;

        [SerializeField]
        private Image itemIcon = null;

        [SerializeField]
        private Text itemCount = null;

        public override void Initialize() {
            base.Initialize();
        }

        public override void UpdateView(IParkOpenCellElement parkOpenCellElement)
        {
            var parkOpenStartRewardListContentViewElement = parkOpenCellElement as ParkOpenStartRewardListContentViewElement;
            Debug.Assert(parkOpenStartRewardListContentViewElement != null, "ParkOpenStartRewardListContentViewElementではありません");

            this.itemName.text = parkOpenStartRewardListContentViewElement.Name;
            this.itemIcon.sprite = parkOpenStartRewardListContentViewElement.Icon;
            this.itemCount.text = parkOpenStartRewardListContentViewElement.Count;
        }
    }    
}