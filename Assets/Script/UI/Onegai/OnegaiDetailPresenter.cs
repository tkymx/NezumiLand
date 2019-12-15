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

        public void SetOnegaiDetail(PlayerOnegaiModel playerOnegaiModel) {
            this.onegaiDetailView.UpdateCell(
                playerOnegaiModel.OnegaiModel.Title,
                playerOnegaiModel.OnegaiModel.Detail,
                playerOnegaiModel.OnegaiModel.Author,
                playerOnegaiModel.HasSchedule (),
                playerOnegaiModel.CloseTime ()
            );
        }
    }    
}