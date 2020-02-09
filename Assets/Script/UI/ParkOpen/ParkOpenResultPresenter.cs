using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class ParkOpenResultPresenter : UiWindowPresenterBase
    {
        [SerializeField]
        private ParkOpenResultView parkOpenResultView = null;

        public void Initialize() {
            this.parkOpenResultView.Initialize();
            this.disposables.Add(parkOpenResultView.OnBackObservable.Subscribe(_ => {
                this.Close();
            }));
            this.Close();
        }

        public void SetContents(ParkOpenResultAmount parkOpenResultAmount) {
            this.parkOpenResultView.UpdateView(
                parkOpenResultAmount.CurrentHeartCount.ToString(),
                parkOpenResultAmount.GoalHeartCount.ToString(),
                parkOpenResultAmount.IsSuccess
            );
        }
    }    
}