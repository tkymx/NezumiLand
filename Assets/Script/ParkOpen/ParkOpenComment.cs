using System;
using UnityEngine;

namespace NL
{
    /// <summary>
    /// 開放中のコメント
    /// </summary>
    public class ParkOpenComment
    {
        // 初回会話
        private readonly uint InitialConversationId = 50000001;

        private readonly ParkOpenInitialCommentPresenter parkOpenInitialCommentPresenter;
        private readonly ConversationPresenter parkOpenCommentConversationPresenter;

        private readonly ConversationModel parkOpenInitialConversationModel;

        public ParkOpenComment(IConversationRepository conversationRepository)
        {
            this.parkOpenInitialCommentPresenter = GameManager.Instance.GameUIManager.ParkOpenInitialCommentPresenter;
            this.parkOpenCommentConversationPresenter = GameManager.Instance.GameUIManager.ParkOpenCommentConversationPresernter;

            this.parkOpenInitialConversationModel = conversationRepository.Get(InitialConversationId);
            Debug.Assert(this.parkOpenInitialConversationModel != null, "初回の会話が取得できません" + InitialConversationId.ToString());
        }

        public IObservable<int> StartInitialComment()
        {
            this.parkOpenInitialCommentPresenter.Show();
            return this.parkOpenInitialCommentPresenter.StartOpen()
                .SelectMany(_ => {
                    // 会話開始
                    this.parkOpenCommentConversationPresenter.Show();
                    this.parkOpenCommentConversationPresenter.StartConversation(parkOpenInitialConversationModel);
                    return this.parkOpenCommentConversationPresenter.OnEndConversation;
                })
                .SelectMany(_ => {
                    return this.parkOpenInitialCommentPresenter.StartClose();
                })
                .Do(_ => {
                    this.parkOpenInitialCommentPresenter.Close();
                });
        }
    }
}