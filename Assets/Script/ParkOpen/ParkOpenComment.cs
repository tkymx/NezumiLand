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
        private readonly uint BeforePromotionConversationId = 50000001;
        private readonly uint AfterPromotionConversationId = 50000001;

        private readonly IConversationRepository conversationRepository;

        public ParkOpenComment(IConversationRepository conversationRepository)
        {
            this.conversationRepository = conversationRepository;
        }

        public IObservable<int> StartInitialComment()
        {
            var parkOpenInitialConversationModel = this.conversationRepository.Get(InitialConversationId);
            Debug.Assert(parkOpenInitialConversationModel != null, "初回の会話が取得できません" + InitialConversationId.ToString());

            GameManager.Instance.GameUIManager.ParkOpenInitialCommentPresenter.Show();
            return GameManager.Instance.GameUIManager.ParkOpenInitialCommentPresenter.StartOpen()
                .SelectMany(_ => {
                    // 会話開始
                    GameManager.Instance.GameUIManager.ParkOpenCommentConversationPresernter.Show();
                    GameManager.Instance.GameUIManager.ParkOpenCommentConversationPresernter.StartConversation(parkOpenInitialConversationModel);
                    return GameManager.Instance.GameUIManager.ParkOpenCommentConversationPresernter.OnEndConversation;
                })
                .SelectMany(_ => {
                    return GameManager.Instance.GameUIManager.ParkOpenInitialCommentPresenter.StartClose();
                })
                .Do(_ => {
                    GameManager.Instance.GameUIManager.ParkOpenInitialCommentPresenter.Close();
                });
        }

        public IObservable<int> StartBeforePromotionComment()
        {
            return StartConversation(this.BeforePromotionConversationId);
        }

        public IObservable<int> StartAfterPromotionComment()
        {
            return StartConversation(this.AfterPromotionConversationId);            
        }

        private IObservable<int> StartConversation(uint conversationId)
        {
            var conversationModel = this.conversationRepository.Get(BeforePromotionConversationId);
            Debug.Assert(conversationModel != null, "会話が取得できません" + BeforePromotionConversationId.ToString());

            GameManager.Instance.GameUIManager.ParkOpenCommentConversationPresernter.Show();
            GameManager.Instance.GameUIManager.ParkOpenCommentConversationPresernter.StartConversation(conversationModel);
            return GameManager.Instance.GameUIManager.ParkOpenCommentConversationPresernter.OnEndConversation;
        }
    }
}