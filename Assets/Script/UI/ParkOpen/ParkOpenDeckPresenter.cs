using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class ParkOpeDeckPresenter : UiWindowPresenterBase
    {
        [SerializeField]
        private ParkOpenCardPresenter parkOpenCardPresenter1 = null;
        private ParkOpenCardPresenter parkOpenCardPresenter2 = null;
        private ParkOpenCardPresenter parkOpenCardPresenter3 = null;

        public void Initialize() {
            this.parkOpenCardPresenter1.Initialize();
            this.parkOpenCardPresenter2.Initialize();
            this.parkOpenCardPresenter3.Initialize();
            this.Close();
        }

        public void SetContents(PlayerParkOpenDeckModel playerParkOpenDeckModel) {
            ParkOpeDeckPresenter.SetContents(this.parkOpenCardPresenter1, playerParkOpenDeckModel.PlayerParkOpenCardModel1);
            ParkOpeDeckPresenter.SetContents(this.parkOpenCardPresenter2, playerParkOpenDeckModel.PlayerParkOpenCardModel2);
            ParkOpeDeckPresenter.SetContents(this.parkOpenCardPresenter3, playerParkOpenDeckModel.PlayerParkOpenCardModel3);
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