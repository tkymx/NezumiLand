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
        private readonly RewardModel rewardModel = null;
        private IDisposable disposable = null;

        private TypeObservable<int> onEndReceiveObservable = new TypeObservable<int>();
        public TypeObservable<int> OnEndReceiveObservable => onEndReceiveObservable;

        public RewardReceiver (RewardModel rewardModel) {
            this.rewardModel = rewardModel;
        }

        public void ReceiveRewardAndShowModel() {

            // 報酬を受け取り
            foreach (var rewardAmount in rewardModel.RewardAmounts)
            {
                rewardAmount.Receive();                
            }

            var currentRewardCount = 0;
            GameManager.Instance.GameUIManager.RewardPresenter.SetRewardAmount(rewardModel.RewardAmounts[currentRewardCount]);
            GameManager.Instance.GameUIManager.RewardPresenter.Show();
            this.disposable = GameManager.Instance.GameUIManager.RewardPresenter.OnClose.Subscribe(_ => {

                // 次の報酬に移動
                currentRewardCount++;

                // 報酬がまだあるかどうか？
                if (currentRewardCount < rewardModel.RewardAmounts.Count) {

                    // 次のモデルの受け取り画面を開く
                    GameManager.Instance.GameUIManager.RewardPresenter.SetRewardAmount(rewardModel.RewardAmounts[currentRewardCount]);
                    GameManager.Instance.GameUIManager.RewardPresenter.Show();
                } else {

                    // 終了する
                    if (this.disposable != null) {
                        this.disposable.Dispose();
                    }
                    onEndReceiveObservable.Execute(0);
                }
            });
        }
    }
}