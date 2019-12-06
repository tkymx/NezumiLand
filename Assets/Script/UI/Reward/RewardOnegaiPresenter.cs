using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace  NL
{
    public class RewardOnegaiPresenter : RewardWindowPresenterBase
    {
        private IOnegaiRepository onegaiRepository = null;

        [SerializeField]
        RewardOnegaiView rewardOnegaiView = null;

        private OnegaiModel onegaiModel = null;

        public void Initialize(OnegaiRepository onegaiRepository) {
            this.onegaiRepository = onegaiRepository;
            this.rewardOnegaiView.Initialize();
            this.disposables.Add(this.rewardOnegaiView.OnClickCloseObservable.Subscribe(_ => {
                this.Close();
            }));
            this.disposables.Add(this.rewardOnegaiView.OnClickDetailObservable.Subscribe(_ => {
                if (this.onegaiModel != null) {
                    GameManager.Instance.GameUIManager.OnegaiDetailPresenter.SetOnegaiDetail(onegaiModel);
                    GameManager.Instance.GameUIManager.OnegaiDetailPresenter.Show();
                }
            }));
            this.Close();
        }

        public override void SetRewardAmount(IRewardAmount rewardAmount) {
            var onegaiRewardAmount = rewardAmount as OnegaiRewardAmount;
            Debug.Assert(onegaiRewardAmount != null, "onegaiRewardAmountがnullです");
            this.onegaiModel = this.onegaiRepository.Get(onegaiRewardAmount.OnegaiId);
        }
    }   
}