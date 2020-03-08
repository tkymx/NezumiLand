using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace NL {

    /// <summary>
    /// 報酬の受け取りと受け取りモーダルを表示する
    /// </summary>
    public class RewardReceiver
    {
        private readonly List<IRewardAmount> rewardAmounts = null;
        private IDisposable disposable = null;

        private TypeObservable<int> onEndReceiveObservable = new TypeObservable<int>();
        public TypeObservable<int> OnEndReceiveObservable => onEndReceiveObservable;

        public RewardReceiver (List<IRewardAmount> rewardAmounts) {
            this.rewardAmounts = rewardAmounts;
        }

        public RewardReceiver (RewardModel rewardModel) {
            this.rewardAmounts = rewardModel.RewardAmounts;
        }

        public RewardReceiver (PlayerEventModel playerEventModel) {
            this.rewardAmounts = playerEventModel.EventModel.RewardModel.RewardAmounts;
        }

        public void ReceiveRewardAndShowModel() {

            // 報酬がなければ終了
            if (this.rewardAmounts.Count <= 0) {
                onEndReceiveObservable.Execute(0);
                return;                
            }

            // 報酬を受け取り
            foreach (var rewardAmount in this.rewardAmounts)
            {
                rewardAmount.Receive();                
            }

            // 報酬の受け取り
            this.showRewardWindow(this.rewardAmounts.Count);
        }

        private void showRewardWindow(int maxRecordCount, int currentRewardCount = 0) {
            if (currentRewardCount >= maxRecordCount) {
                this.onEndReceiveObservable.Execute(0);
                return;
            }
            var rewardAmount = this.rewardAmounts[currentRewardCount];
            var rewardWindowPresenterBase = GetRewardWindowPresenterBase(rewardAmount);
            rewardWindowPresenterBase.SetRewardAmount(rewardAmount);
            rewardWindowPresenterBase.Show();
            this.disposable = rewardWindowPresenterBase.OnClose.Subscribe(_ => {
                if (this.disposable != null) {
                    this.disposable.Dispose();
                }
                showRewardWindow(maxRecordCount, currentRewardCount+1);
            });
        }

        private RewardWindowPresenterBase GetRewardWindowPresenterBase(IRewardAmount rewardAmount) {
            if (rewardAmount.RewardType == RewardType.Onegai) {
                return GameManager.Instance.GameUIManager.RewardOnegaiPresenter;
            }
            if (rewardAmount.RewardType == RewardType.Mono) {
                return GameManager.Instance.GameUIManager.RewardMonoInfoPresenter;
            }
            if (rewardAmount.RewardType == RewardType.ParkOpenGroup) {
                return GameManager.Instance.GameUIManager.RewardParkOpenGroupPresenter;
            }
            return GameManager.Instance.GameUIManager.RewardPresenter;
        }
    }
}