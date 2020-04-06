using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    /// <summary>
    /// 開園機能のはじめの白背景にコメントの部分のプレゼンター
    /// </summary>
    public class ParkOpenInitialCommentPresenter : UiWindowPresenterBase
    {
        [SerializeField]
        private ParkOpenInitialCommentView parkOpenInitialCommentView = null;

        [SerializeField]
        private ConversationPresenter parkOpenConversationPresenter = null;

        // 終了のObservable
        public TypeObservable<int> OnEndObservable { get; private set; }

        public void Initialize() {
            this.OnEndObservable = new TypeObservable<int>();
            parkOpenInitialCommentView.Initialize();
            parkOpenConversationPresenter.Initialize();

            this.Close();
        }

        public void UpdateByFrame()
        {
            this.parkOpenInitialCommentView.UpdateByFrame();
        }

        public void StartComment(ConversationModel conversationModel) {

            this.Show();

            // フェードイン開始
            this.parkOpenInitialCommentView.StartOpen();
            this.disposables.Add(this.parkOpenInitialCommentView.OnEndOpenAnimation
                .SelectMany(_ => {
                    // 会話開始
                    this.parkOpenConversationPresenter.Show();
                    this.parkOpenConversationPresenter.StartConversation(conversationModel);
                    return this.parkOpenConversationPresenter.OnEndConversation;
                })
                .SelectMany(_ => {
                    // アニメーション開始
                    this.parkOpenInitialCommentView.StartClose();
                    return this.parkOpenInitialCommentView.OnEndCloseAnimation;
                })
                .Subscribe(_ => {
                    this.Close();
                    this.OnEndObservable.Execute(0);
                }));
        }
    }    
}