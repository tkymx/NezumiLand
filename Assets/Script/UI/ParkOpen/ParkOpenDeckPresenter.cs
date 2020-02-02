using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class ParkOpenDeckPresenter : UiWindowPresenterBase
    {
        [SerializeField]
        private ParkOpenCardPresenter parkOpenCardPresenter1 = null;
        [SerializeField]
        private ParkOpenCardPresenter parkOpenCardPresenter2 = null;
        [SerializeField]
        private ParkOpenCardPresenter parkOpenCardPresenter3 = null;

        public void Initialize() {
            this.parkOpenCardPresenter1.Initialize();
            this.parkOpenCardPresenter2.Initialize();
            this.parkOpenCardPresenter3.Initialize();
            this.Close();
        }

        public void SetContents(PlayerParkOpenDeckModel playerParkOpenDeckModel) {
            ParkOpenDeckPresenter.SetContents(this.parkOpenCardPresenter1, playerParkOpenDeckModel.FirstCardModel);
            ParkOpenDeckPresenter.SetContents(this.parkOpenCardPresenter2, playerParkOpenDeckModel.SecondCardModel);
            ParkOpenDeckPresenter.SetContents(this.parkOpenCardPresenter3, playerParkOpenDeckModel.ThirdCardModel);
        }

        private static void SetContents(ParkOpenCardPresenter parkOpenCardPresenter, PlayerParkOpenCardModel playerParkOpenCardModel)
        {
            if (playerParkOpenCardModel != null) {
                parkOpenCardPresenter.Show();
                parkOpenCardPresenter.SetContents(playerParkOpenCardModel.ParkOpenCardModel);
            } else {
                parkOpenCardPresenter.Close();
            }            
        }
    }    
}