using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class ParkOpenStartPresenter : UiWindowPresenterBase
    {
        [SerializeField]
        private ParkOpenStartView parkOpenStartView = null;

        public TypeObservable<int> OnStartObservable { get; private set; }

        public void Initialize() {
            this.OnStartObservable = new TypeObservable<int>();
            this.parkOpenStartView.Initialize();
            this.disposables.Add(parkOpenStartView.OnStartObservable.Subscribe(_ => {
                this.OnStartObservable.Execute(_);
            }));
            this.disposables.Add(parkOpenStartView.OnBackObservable.Subscribe(_ => {
                this.Close();
            }));
            this.Close();
        }

        public void SetContents(ParkOpenGroupModel parkOpenGroupModel) {
            this.parkOpenStartView.UpdateView(parkOpenGroupModel.ParkOpenGroupViewInfo.GroupName);            
        }
    }    
}