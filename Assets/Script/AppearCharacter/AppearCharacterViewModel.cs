using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {

    /// <summary>
    /// インスタンス化されたキャラクタの行動を管理
    /// </summary>
    public class AppearCharacterViewModel
    {
        private AppearCharacterView appearCharacterView;
        private AppearCharacterModel appearCharacterModel;
        private ConversationModel conversationModel;    /*今は固定だが、後々は抽象化したい*/

        public AppearCharacterViewModel(AppearCharacterView appearCharacterView, AppearCharacterModel appearCharacterModel, ConversationModel conversationModel)
        {
            this.appearCharacterView = appearCharacterView;            
            this.appearCharacterModel = appearCharacterModel;
            this.conversationModel = conversationModel;

            appearCharacterView.OnSelectObservable.Subscribe(_ => {
                // 会話を開始
                GameManager.Instance.GameModeManager.EnqueueChangeModeWithHistory(GameModeGenerator.GenerateConversationMode(this.conversationModel,()=>{
                    GameManager.Instance.GameModeManager.Back();
                    
                    // 終了後に撤退の動作を始める
                    // この動作も抽象化したいが。。。
                    if(GameManager.Instance.DailyAppearCharacterRegistManager.IsRemoveReserve(this)) {
                        GameManager.Instance.DailyAppearCharacterRegistManager.RemoveReserve(this); 
                    }
                }));
            });
        }

        public void UpdateByFrame()
        {
            // なにか行動する
        }

        public void Dispose () 
        {
            Object.DisAppear(appearCharacterView.gameObject);
        }
    }
}