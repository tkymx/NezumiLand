using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace NL
{
    public class ParkOpenStartRewardListHeaderView : ParkOpenStartRewardListCellViewBase
    {
        [SerializeField]
        private Text headerText = null;

        public override void Initialize() {
            base.Initialize();
        }

        public override void UpdateView(IParkOpenCellElement parkOpenCellElement)
        {
            var parkOpenStartRewardListHeaderViewElement = parkOpenCellElement as ParkOpenStartRewardListHeaderViewElement;
            Debug.Assert(parkOpenStartRewardListHeaderViewElement != null, "ParkOpenStartRewardListHeaderViewElement ではありません");

            this.headerText.text = parkOpenStartRewardListHeaderViewElement.HeaderTitle;
        }
    }     
}