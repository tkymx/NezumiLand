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

            appearCharacterView.OnSelect.Subscribe(_ => {
                // タッチされたら会話を流す
            });
        }

        public void UpdateByFrame()
        {
            // なにか行動する
        }
    }
}