using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace NL {
    public class ReceiveRewardMode : IGameMode {

        private IDisposable receiveRewardDisposable = null;
        private RewardModel rewardModel = null;
        private RewardReceiver rewardReceiver = null;

        public ReceiveRewardMode(RewardModel rewardModel)
        {
            this.receiveRewardDisposable = null;
            this.rewardModel = rewardModel;
            this.rewardReceiver = new RewardReceiver(this.rewardModel);
        }

        public void OnEnter () { 
            this.receiveRewardDisposable = rewardReceiver.OnEndReceiveObservable.Subscribe(_ => {
                GameManager.Instance.GameModeManager.Back();
            });
            rewardReceiver.ReceiveRewardAndShowModel();
        }
        public void OnUpdate () {
        }
        public void OnExit () { 
            if (this.receiveRewardDisposable != null) {
                this.receiveRewardDisposable.Dispose();
            } 
        }
    }
}
