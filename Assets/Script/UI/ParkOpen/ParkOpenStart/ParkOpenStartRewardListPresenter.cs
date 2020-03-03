using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace NL {
    public class ParkOpenStartRewardListPresenter : ListPresenterBase<IParkOpenCellElement, ParkOpenStartRewardListCellViewBase> {

        [SerializeField]
        private GameObject cellHeaderPrefab = null;

        [SerializeField]
        private GameObject cellSpecialHeaderPrefab = null;

        [SerializeField]
        private GameObject cellContentPrefab = null;
        
        public void SetRewardContents(PlayerParkOpenGroupModel playerParkOpenGroupModel)
        {
            List<IParkOpenCellElement> parkOpenRewardCellElements = new List<IParkOpenCellElement>();

            if (playerParkOpenGroupModel.IsSpecial)
            {
                // 特別報酬
                parkOpenRewardCellElements.Add(new ParkOpenStartRewardListHeaderViewElement(cellSpecialHeaderPrefab, "特別報酬"));
                parkOpenRewardCellElements.AddRange(
                    playerParkOpenGroupModel.ParkOpenGroupModel.SpecialClearReward.RewardAmounts
                        .Select(rewardAmount => new ParkOpenStartRewardListContentViewElement(cellContentPrefab, rewardAmount.Name, rewardAmount.Image, rewardAmount.Amount.ToString()))
                        .ToList());
            }
            // 初回報酬
            parkOpenRewardCellElements.Add(new ParkOpenStartRewardListHeaderViewElement(cellHeaderPrefab, "初回報酬"));
            parkOpenRewardCellElements.AddRange(
                playerParkOpenGroupModel.ParkOpenGroupModel.FirstClearReward.RewardAmounts
                    .Select(rewardAmount => new ParkOpenStartRewardListContentViewElement(cellContentPrefab, rewardAmount.Name, rewardAmount.Image, rewardAmount.Amount.ToString()))
                    .ToList());
            // 通常報酬
            parkOpenRewardCellElements.Add(new ParkOpenStartRewardListHeaderViewElement(cellHeaderPrefab, "通常報酬"));
            parkOpenRewardCellElements.AddRange(
                playerParkOpenGroupModel.ParkOpenGroupModel.ClearReward.RewardAmounts
                    .Select(rewardAmount => new ParkOpenStartRewardListContentViewElement(cellContentPrefab, rewardAmount.Name, rewardAmount.Image, rewardAmount.Amount.ToString()))
                    .ToList());
            // クリアハート報酬
            foreach (var parkOpenRewardAmount in playerParkOpenGroupModel.ParkOpenGroupModel.ObtainedHeartRewards)
            {
                parkOpenRewardCellElements.Add(new ParkOpenStartRewardListHeaderViewElement(cellHeaderPrefab, string.Format("ハート達成報酬（{0}）", parkOpenRewardAmount.ObtainHeartCount)));
                parkOpenRewardCellElements.AddRange(
                    parkOpenRewardAmount.Reward.RewardAmounts
                        .Select(rewardAmount => new ParkOpenStartRewardListContentViewElement(cellContentPrefab, rewardAmount.Name, rewardAmount.Image, rewardAmount.Amount.ToString()))
                        .ToList());
            }
            
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