using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace  NL
{
    public class RewardMonoPresenter : RewardWindowPresenterBase
    {
        private IMonoInfoRepository monoInfoRepository = null;

        [SerializeField]
        RewardMonoView rewardMonoInfoView = null;

        private MonoInfo monoInfoModel = null;

        public void Initialize(IMonoInfoRepository monoInfoRepository) {
            this.monoInfoRepository = monoInfoRepository;
            this.rewardMonoInfoView.Initialize();
            this.disposables.Add(this.rewardMonoInfoView.OnClickCloseObservable.Subscribe(_ => {
                this.Close();
            }));
            this.Close();
        }

        public override void SetRewardAmount(IRewardAmount rewardAmount) {
            var monoRewardAmount = rewardAmount as MonoRewardAmount;
            Debug.Assert(monoRewardAmount != null, "MonoRewardAmountがnullです");
            this.monoInfoModel = this.monoInfoRepository.Get(monoRewardAmount.MonoInfoId);
            this.rewardMonoInfoView.UpdateView(this.monoInfoModel.Name);
        }
    }   
}