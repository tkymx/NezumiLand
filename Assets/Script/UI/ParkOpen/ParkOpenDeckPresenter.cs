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

        private ParkOpenCardPresenter GetParkOpenCardPresenter(PlayerParkOpenDeckModel.CountType countType)
        {
            if (countType == PlayerParkOpenDeckModel.CountType.First)
            {
                return parkOpenCardPresenter1;
            }
            if (countType == PlayerParkOpenDeckModel.CountType.Second)
            {
                return parkOpenCardPresenter2;
            }
            if (countType == PlayerParkOpenDeckModel.CountType.Third)
            {
                return parkOpenCardPresenter3;
            }

            return null;
        }

        public void Initialize() {
            this.parkOpenCardPresenter1.Initialize();
            this.parkOpenCardPresenter2.Initialize();
            this.parkOpenCardPresenter3.Initialize();
            this.Close();

            this.SetTouchEvent(PlayerParkOpenDeckModel.CountType.First);            
            this.SetTouchEvent(PlayerParkOpenDeckModel.CountType.Second);            
            this.SetTouchEvent(PlayerParkOpenDeckModel.CountType.Third);            
        }

        public void SetTouchEvent(PlayerParkOpenDeckModel.CountType countType)
        {
            var playerParkOpenDeckModel = GetParkOpenCardPresenter(countType);
            this.disposables.Add(playerParkOpenDeckModel.OnTouchCardObservable.Subscribe(_ => {
                CancelAnother(countType);
                playerParkOpenDeckModel.TogglePrepare();
            }));
            this.disposables.Add(playerParkOpenDeckModel.OnCancelObservable.Subscribe(_ => {
                playerParkOpenDeckModel.CancelPrepare();
            }));
            this.disposables.Add(playerParkOpenDeckModel.OnUseObservable.Subscribe(_ => {
                // カードを消費する
            }));
        }

        public void CancelAnother(PlayerParkOpenDeckModel.CountType countType)
        {
            if (countType != PlayerParkOpenDeckModel.CountType.First) {
                GetParkOpenCardPresenter(PlayerParkOpenDeckModel.CountType.First).CancelPrepare();
            }
            if (countType != PlayerParkOpenDeckModel.CountType.Second) {
                GetParkOpenCardPresenter(PlayerParkOpenDeckModel.CountType.Second).CancelPrepare();
            }
            if (countType != PlayerParkOpenDeckModel.CountType.Third) {
                GetParkOpenCardPresenter(PlayerParkOpenDeckModel.CountType.Third).CancelPrepare();
            }
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