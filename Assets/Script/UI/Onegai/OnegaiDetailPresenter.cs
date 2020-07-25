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

        public TypeObservable<int> OnEntry { get; private set; }

        public TypeObservable<int> OnCancel { get; private set; }

        public void Initialize() {
            this.OnEntry = new TypeObservable<int>();
            this.OnCancel = new TypeObservable<int>();
            this.onegaiDetailView.Initialize();
            this.disposables.Add(this.onegaiDetailView.OnBack.Subscribe(_ => {
                this.Close();
            }));
            this.disposables.Add(this.onegaiDetailView.OnEntry.Subscribe(_ => {
                this.OnEntry.Execute(0);
            }));
            this.disposables.Add(this.onegaiDetailView.OnCancel.Subscribe(_ => {
                this.OnCancel.Execute(0);
            }));
            this.Close();
        }

        public void SetOnegaiDetail(PlayerOnegaiModel playerOnegaiModel) {
            this.onegaiDetailView.UpdateCell(
                playerOnegaiModel.OnegaiModel.Title,
                playerOnegaiModel.OnegaiModel.Detail,
                "依頼人 : " + playerOnegaiModel.OnegaiModel.Author,
                playerOnegaiModel.HasSchedule (),
                playerOnegaiModel.CloseTime (),
                playerOnegaiModel.OnegaiModel.Satisfaction.ToString());
            this.onegaiDetailView.SetCnacelButtonEnabled(false);
            this.onegaiDetailView.SetEntryButtonEnabled(false);
        }

        public void SetOnegaiDetailWithEntry(PlayerOnegaiModel playerOnegaiModel) {
            this.SetOnegaiDetail(playerOnegaiModel);
            this.onegaiDetailView.SetCnacelButtonEnabled(false);
            this.onegaiDetailView.SetEntryButtonEnabled(true);
        }

        public void SetOnegaiDetailWithCancel(PlayerOnegaiModel playerOnegaiModel) {
            this.SetOnegaiDetail(playerOnegaiModel);
            this.onegaiDetailView.SetCnacelButtonEnabled(true);
            this.onegaiDetailView.SetEntryButtonEnabled(false);
        }
    }    
}