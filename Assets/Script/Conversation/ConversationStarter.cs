using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public class ConversationStarter
    {
        /// <summary>
        /// 会話を開始
        /// </summary>
        /// <returns>終了のObservable</returns>
        public static TypeObservable<int> StartConversation(ConversationModel conversationModel) {
            GameManager.Instance.GameUIManager.ConversationPresenter.StartConversation(conversationModel);
            return GameManager.Instance.GameUIManager.ConversationPresenter.OnEndConversation;
        }
    }
}