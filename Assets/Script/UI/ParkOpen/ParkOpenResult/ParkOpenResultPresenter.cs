using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class ParkOpenResultPresenter : UiWindowPresenterBase
    {
        [SerializeField]
        private ParkOpenResultView parkOpenResultView = null;

        [SerializeField]
        private ParkOpenResultObtainRewardInfoPresenter parkOpenResultObtainRewardInfoPresenter = null;

        [SerializeField]
        private HeartPresenter heartPresenter = null;

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
            this.parkOpenResultObtainRewardInfoPresenter.SetElement(parkOpenResultAmount.SpecialRewardResults);
            this.heartPresenter.UpdateHeart(parkOpenResultAmount.CurrentHeartCount, parkOpenResultAmount.TargetPlayerGroupModel.ParkOpenGroupModel.MaxHeartCount, parkOpenResultAmount.GoalHeartCount);
        }
    }    
}