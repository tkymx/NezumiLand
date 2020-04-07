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

        public void Initialize() {
            parkOpenInitialCommentView.Initialize();
            this.Close();
        }

        public void UpdateByFrame()
        {
            this.parkOpenInitialCommentView.UpdateByFrame();
        }

        public TypeObservable<int> StartOpen()
        {
            this.parkOpenInitialCommentView.StartOpen();
            return this.parkOpenInitialCommentView.OnEndOpenAnimation;
        }

        public TypeObservable<int> StartClose()
        {
            this.parkOpenInitialCommentView.StartClose();
            return this.parkOpenInitialCommentView.OnEndCloseAnimation;
        }
    }    
}