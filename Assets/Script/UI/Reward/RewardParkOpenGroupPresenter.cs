using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace  NL
{
    public class RewardParkOpenGroupPresenter : RewardWindowPresenterBase
    {
        private IParkOpenGroupRepository parkOpenGroupRepository = null;

        [SerializeField]
        RewardParkOpenGroupView rewardParkOpenGroupView = null;

        public void Initialize(IParkOpenGroupRepository parkOpenGroupRepository) {
            this.parkOpenGroupRepository = parkOpenGroupRepository;
            this.rewardParkOpenGroupView.Initialize();
            this.disposables.Add(this.rewardParkOpenGroupView.OnClickCloseObservable.Subscribe(_ => {
                this.Close();
            }));
            this.Close();
        }

        public override void SetRewardAmount(IRewardAmount rewardAmount) {
            var monoRewardAmount = rewardAmount as ParkOpenGroupRewardAmount;
            Debug.Assert(monoRewardAmount != null, "ParkOpenGroupRewardAmount がnullです");
            var parkOpenGroupModel = this.parkOpenGroupRepository.Get(monoRewardAmount.ParkOpenGroupId);
            this.rewardParkOpenGroupView.UpdateView(parkOpenGroupModel.ParkOpenGroupViewInfo.GroupName);
        }
    }   
}