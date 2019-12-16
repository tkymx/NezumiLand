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
        private AppearCharacterView appearCharacterView;
        private AppearCharacterModel appearCharacterModel;
        private ConversationModel conversationModel;    /*今は固定だが、後々は抽象化したい*/
        private RewardModel rewardModel;

        private List<IDisposable> disposables = new List<IDisposable>();

        private bool isReceiveReward = false;

        public AppearCharacterViewModel(AppearCharacterView appearCharacterView, AppearCharacterModel appearCharacterModel, ConversationModel conversationModel, RewardModel rewardModel)
        {
            this.appearCharacterView = appearCharacterView;            
            this.appearCharacterModel = appearCharacterModel;
            this.conversationModel = conversationModel;
            this.rewardModel = rewardModel;

            // キャラクタがタップされた時
            disposables.Add(appearCharacterView.OnSelectObservable
                .SelectMany(_ => {
                    var conversationMode =  GameModeGenerator.GenerateConversationMode(this.conversationModel);
                    GameManager.Instance.GameModeManager.EnqueueChangeModeWithHistory(conversationMode);
                    return GameManager.Instance.GameModeManager.GetModeEndObservable(conversationMode);
                })
                .SelectMany(_ => {
                    if (this.isReceiveReward) {
                        return new ImmediatelyObservable<int>(_);
                    }
                    this.isReceiveReward = true;
                    var rewardMode = GameModeGenerator.GenerateReceiveRewardMode(this.rewardModel);
                    GameManager.Instance.GameModeManager.EnqueueChangeModeWithHistory(rewardMode);
                    return GameManager.Instance.GameModeManager.GetModeEndObservable(rewardMode);
                })
                .Subscribe(_ => {
                    // TODO 終了後に撤退の動作を始める
                    // 予約から消す必要があれば消す
                    if(GameManager.Instance.DailyAppearCharacterRegistManager.IsRemoveReserve(this)) {
                        GameManager.Instance.DailyAppearCharacterRegistManager.RemoveReserve(this); 
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