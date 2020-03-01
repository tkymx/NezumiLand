using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace NL {
    public class ParkOpenStartRewardListPresenter : ListPresenterBase<IParkOpenCellElement, ParkOpenStartRewardListCellViewBase> {

        [SerializeField]
        private GameObject cellHeaderPrefab = null;
        
        [SerializeField]
        private GameObject cellContentPrefab = null;
        
        public void SetRewardContents(ParkOpenGroupModel parkOpenGroupModel)
        {
            List<IParkOpenCellElement> parkOpenRewardCellElements = new List<IParkOpenCellElement>();
            // 通常報酬
            parkOpenRewardCellElements.Add(new ParkOpenStartRewardListHeaderViewElement(cellHeaderPrefab, "通常報酬"));
            parkOpenRewardCellElements.AddRange(
                parkOpenGroupModel.ClearReward.RewardAmounts
                    .Select(rewardAmount => new ParkOpenStartRewardListContentViewElement(cellContentPrefab, rewardAmount.Name, rewardAmount.Image, rewardAmount.Amount.ToString()) as IParkOpenCellElement)
                    .ToList());
            this.SetElement(parkOpenRewardCellElements);
        }

        protected override GameObject onGetCellPrefab(IParkOpenCellElement element)
        {
            return element.MainPrefab;
        }

        protected override void onReloadCell (IParkOpenCellElement element, ParkOpenStartRewardListCellViewBase cellView) {
            cellView.UpdateView(element);
        }
    }
}