using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

        /// <summary>
        /// カードがタップされたときのイベント
        /// </summary>
        /// <value></value>
        public TypeObservable<PlayerParkOpenDeckModel.CountType> OnTouchUseCardObservable { get; private set; }

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

            this.OnTouchUseCardObservable = new TypeObservable<PlayerParkOpenDeckModel.CountType>();

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
            var playerParkOpenCardPresenter = GetParkOpenCardPresenter(countType);
            this.disposables.Add(playerParkOpenCardPresenter.OnTouchCardObservable.Subscribe(_ => {
                CancelAnother(countType);
                playerParkOpenCardPresenter.TogglePrepare();
            }));
            this.disposables.Add(playerParkOpenCardPresenter.OnCancelObservable.Subscribe(_ => {
                playerParkOpenCardPresenter.CancelPrepare();
            }));
            this.disposables.Add(playerParkOpenCardPresenter.OnUseObservable.Subscribe(_ => {
                this.OnTouchUseCardObservable.Execute(countType);
            }));
        }

        /// <summary>
        /// 指定カード以外をキャンセルする
        /// </summary>
        /// <param name="countType"></param>
        private void CancelAnother(PlayerParkOpenDeckModel.CountType countType)
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

        public void ResetCard()
        {
            parkOpenCardPresenter1.ResetCard();
            parkOpenCardPresenter2.ResetCard();
            parkOpenCardPresenter3.ResetCard();
        }

        public void UseCard(PlayerParkOpenDeckModel.CountType countType)
        {
            var parkOpenCardPresenter = GetParkOpenCardPresenter(countType);
            parkOpenCardPresenter.UseCard();
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