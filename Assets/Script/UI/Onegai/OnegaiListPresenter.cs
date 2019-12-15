using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace NL {
    public class OnegaiListPresenter : ListPresenterBase<PlayerOnegaiModel, OnegaiListCellView> {

        private TypeObservable<PlayerOnegaiModel> onCellClick = null;
        public TypeObservable<PlayerOnegaiModel> OnCellClick => onCellClick;

        public void Initialize() {
            this.onCellClick = new TypeObservable<PlayerOnegaiModel>();
        }

        protected override void onReloadCell (PlayerOnegaiModel element, OnegaiListCellView cellView) {
            cellView.UpdateCell (
                element.OnegaiModel.Title,
                element.OnegaiModel.Detail,
                element.OnegaiState == OnegaiState.Clear,
                element.HasSchedule (),
                element.CloseTime ());
            
            this.disposables.Add(cellView.OnClick.Subscribe(_ => {
                this.onCellClick.Execute(element);                
            }));
        }
    }
}