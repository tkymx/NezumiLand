using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace NL
{
    public class OnegaiDetailPresenter : UiWindowPresenterBase
    {
        [SerializeField]
        private OnegaiDetailView onegaiDetailView = null;

        public void Initialize() {
            this.onegaiDetailView.Initialize();
            this.disposables.Add(this.onegaiDetailView.OnBack.Subscribe(_ => {
                this.Close();
            }));
            this.Close();
        }

        public void SetOnegaiDetail(OnegaiModel onegaiModel) {
            this.onegaiDetailView.UpdateCell(
                onegaiModel.Title,
                onegaiModel.Detail,
                onegaiModel.Author
            );
        }
    }    
}