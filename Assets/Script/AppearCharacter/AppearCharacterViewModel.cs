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

        private List<IDisposable> disposables = new List<IDisposable>();

        public AppearCharacterViewModel(AppearCharacterView appearCharacterView, AppearCharacterModel appearCharacterModel, ConversationModel conversationModel)
        {
            this.appearCharacterView = appearCharacterView;            
            this.appearCharacterModel = appearCharacterModel;
            this.conversationModel = conversationModel;

            // キャラクタがタップされた時
            disposables.Add(appearCharacterView.OnSelectObservable
                .SelectMany(_ => {
                    // 会話モード
                    var conversationMode =  GameModeGenerator.GenerateConversationMode(this.conversationModel);
                    // 会話を開始
                    GameManager.Instance.GameModeManager.EnqueueChangeModeWithHistory(conversationMode);
                    // 会話の終了を取得
                    return GameManager.Instance.GameModeManager.GetModeEndObservable(conversationMode);
                })
                .Subscribe(_ => {
                    // 終了後に撤退の動作を始める
                    // この動作も抽象化したいが。。。
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