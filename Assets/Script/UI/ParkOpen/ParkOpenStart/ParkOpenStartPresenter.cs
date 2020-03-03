using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class ParkOpenStartPresenter : UiWindowPresenterBase
    {
        [SerializeField]
        private ParkOpenStartView parkOpenStartView = null;

        [SerializeField]
        private ParkOpenStartRewardListPresenter parkOpenStartRewardListPresenter = null;

        public TypeObservable<bool> OnIsStartObservable { get; private set; }

        public void Initialize() {
            this.OnIsStartObservable = new TypeObservable<bool>();
            this.parkOpenStartView.Initialize();
            this.disposables.Add(parkOpenStartView.OnStartObservable.Subscribe(_ => {
                this.OnIsStartObservable.Execute(true);
                this.Close();
            }));
            this.disposables.Add(parkOpenStartView.OnBackObservable.Subscribe(_ => {
                this.OnIsStartObservable.Execute(false);
                this.Close();
            }));
            this.Close();
        }

        public void SetContents(PlayerParkOpenGroupModel playerParkOpenGroupModel) {
            this.parkOpenStartView.UpdateView(
                playerParkOpenGroupModel.ParkOpenGroupModel.ParkOpenGroupViewInfo.GroupName,
                playerParkOpenGroupModel.ParkOpenGroupModel.ParkOpenGroupViewInfo.GroupDescription,
                playerParkOpenGroupModel.ParkOpenGroupModel.GoalHeartCount.ToString());
            this.parkOpenStartRewardListPresenter.SetRewardContents(playerParkOpenGroupModel);
        }
    }    
}