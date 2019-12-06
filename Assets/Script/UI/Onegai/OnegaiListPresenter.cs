using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace NL {
    public class OnegaiListPresenter : ListPresenterBase<PlayerOnegaiModel, OnegaiListCellView> {

        private TypeObservable<PlayerOnegaiModel> onCellClick = null;
        public TypeObservable<PlayerOnegaiModel> OnCellClick => onCellClick;

        List<IDisposable> cellCLickDisposables = null;

        public void Initialize() {
            this.onCellClick = new TypeObservable<PlayerOnegaiModel>();
            this.cellCLickDisposables = new List<IDisposable>();            
        }

        protected override void onReloadCell (PlayerOnegaiModel element, OnegaiListCellView cellView) {
            cellView.UpdateCell (
                element.OnegaiModel.Title,
                element.OnegaiModel.Detail,
                element.OnegaiState == OnegaiState.Clear);
            
            var disposable = cellView.OnClick.Subscribe(_ => {
                this.onCellClick.Execute(element);                
            });
            this.cellCLickDisposables.Add(disposable);
        }

        private void OnDestroy() {
            if (this.cellCLickDisposables != null) {
                foreach (var cellCLickDisposable in cellCLickDisposables)
                {
                    cellCLickDisposable.Dispose();
                }
            }
         }
    }
}