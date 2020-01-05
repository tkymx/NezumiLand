using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace NL {

    /// <summary>
    /// インスタンス化されたキャラクタの行動を管理
    /// </summary>
    public class AppearCharacterViewModel
    {
        public PlayerAppearCharacterViewModel PlayerAppearCharacterViewModel { get; private set; }
        private AppearCharacterView appearCharacterView;

        private List<IDisposable> disposables = new List<IDisposable>();

        public AppearCharacterViewModel(AppearCharacterView appearCharacterView, PlayerAppearCharacterViewModel playerAppearCharacterViewModel)
        {
            this.appearCharacterView = appearCharacterView;            
            this.PlayerAppearCharacterViewModel = playerAppearCharacterViewModel;

            // キャラクタがタップされた時
            disposables.Add(appearCharacterView.OnSelectObservable
                .SelectMany(_ => {
                    var conversationMode =  GameModeGenerator.GenerateConversationMode(this.PlayerAppearCharacterViewModel.PlayerAppearCharacterReserveModel.ConversationModel);
                    GameManager.Instance.GameModeManager.EnqueueChangeModeWithHistory(conversationMode);
                    return GameManager.Instance.GameModeManager.GetModeEndObservable(conversationMode);
                })
                .SelectMany(_ => {
                    if (this.PlayerAppearCharacterViewModel.PlayerAppearCharacterReserveModel.RewardModel == null) {
                        return new ImmediatelyObservable<int>(_);
                    }
                    if (this.PlayerAppearCharacterViewModel.IsReceiveReward) {
                        return new ImmediatelyObservable<int>(_);
                    }
                    if (this.PlayerAppearCharacterViewModel.PlayerAppearCharacterReserveModel.RewardModel.RewardAmounts.Count <= 0) {
                        return new ImmediatelyObservable<int>(_);
                    }                    

                    // 受け取り済みにする
                    GameManager.Instance.AppearCharacterManager.ToReeiveRewards(this.PlayerAppearCharacterViewModel);

                    var rewardMode = GameModeGenerator.GenerateReceiveRewardMode(this.PlayerAppearCharacterViewModel.PlayerAppearCharacterReserveModel.RewardModel);
                    GameManager.Instance.GameModeManager.EnqueueChangeModeWithHistory(rewardMode);
                    return GameManager.Instance.GameModeManager.GetModeEndObservable(rewardMode);
                })
                .Subscribe(_ => {
                    // TODO 終了後に撤退の動作を始める
                    // 予約から消す必要があれば消す
                    if(GameManager.Instance.DailyAppearCharacterRegistManager.IsRemoveReserve(this.PlayerAppearCharacterViewModel.PlayerAppearCharacterReserveModel)) {
                        GameManager.Instance.DailyAppearCharacterRegistManager.RemoveReserve(this.PlayerAppearCharacterViewModel.PlayerAppearCharacterReserveModel); 
                    }
                }));
        }

        public void UpdateByFrame()
        {
            // なにか行動する
        }

        public void Dispose () 
        {
            Object.DisAppear(appearCharacterView.gameObject);

            // dispose を駆逐
            foreach (var disposable in disposables)
            {
                disposable.Dispose();
            }
            disposables.Clear();
        }
    }
}