using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace  NL
{
    public class RewardOnegaiPresenter : RewardWindowPresenterBase
    {
        private IPlayerOnegaiRepository playerOegaiRepository = null;

        [SerializeField]
        RewardOnegaiView rewardOnegaiView = null;

        private PlayerOnegaiModel playerOnegaiModel = null;

        public void Initialize(IPlayerOnegaiRepository playerOegaiRepository) {
            this.playerOegaiRepository = playerOegaiRepository;
            this.rewardOnegaiView.Initialize();
            this.disposables.Add(this.rewardOnegaiView.OnClickCloseObservable.Subscribe(_ => {
                this.Close();
            }));
            this.disposables.Add(this.rewardOnegaiView.OnClickDetailObservable.Subscribe(_ => {
                if (this.playerOnegaiModel != null) {
                    GameManager.Instance.GameUIManager.OnegaiDetailPresenter.SetOnegaiDetail(playerOnegaiModel);
                    GameManager.Instance.GameUIManager.OnegaiDetailPresenter.Show();
                }
            }));
            this.Close();
        }

        public override void SetRewardAmount(IRewardAmount rewardAmount) {
            var onegaiRewardAmount = rewardAmount as OnegaiRewardAmount;
            Debug.Assert(onegaiRewardAmount != null, "onegaiRewardAmountがnullです");
            this.playerOnegaiModel = this.playerOegaiRepository.GetById(onegaiRewardAmount.OnegaiId);
            this.rewardOnegaiView.UpdateView(this.playerOnegaiModel.OnegaiModel.Title);
        }
    }   
}