using System;
using UnityEngine;

namespace NL
{
    public class ParkOpenCardManager 
    {
        // レポジトリ
        private IPlayerParkOpenRepository playerParkOpenRepository = null;

        // サービス
        private ParkOpenCardCreateService parkOpenCardCreateService = null;
        private ParkOpenDeckCreateService parkOpenDeckCreateService = null;
        private ParkOpenSelectDeckCardService parkOpenSelectDeckCardService = null;
        private ParkOpenSetMainDeckService parkOpenSetMainDeckService = null;
        private ParkOpenUseCardService parkOpenUseCardService = null;

        public ParkOpenCardManager (IPlayerParkOpenRepository playerParkOpenRepository, IPlayerParkOpenCardRepository playerParkOpenCardRepository, IPlayerParkOpenDeckRepository playerParkOpenDeckRepository)
        {
            this.playerParkOpenRepository = playerParkOpenRepository;

            this.parkOpenCardCreateService = new ParkOpenCardCreateService(playerParkOpenCardRepository);
            this.parkOpenDeckCreateService = new ParkOpenDeckCreateService(playerParkOpenDeckRepository);
            this.parkOpenSelectDeckCardService = new ParkOpenSelectDeckCardService(playerParkOpenDeckRepository);
            this.parkOpenSetMainDeckService = new ParkOpenSetMainDeckService(playerParkOpenRepository);
            this.parkOpenUseCardService = new ParkOpenUseCardService(playerParkOpenRepository);
        }

        /// <summary>
        /// カードを獲得する
        /// </summary>
        /// <param name="parkOpenCardModel"></param>
        public void ObtainCard(ParkOpenCardModel parkOpenCardModel)
        {
            this.parkOpenCardCreateService.Execute(parkOpenCardModel);            
        }
        
        /// <summary>
        /// 新規デッキを生成する
        /// </summary>
        public void CreateDeck()
        {
            this.parkOpenDeckCreateService.Execute();
        }

        /// <summary>
        /// メインのデッキをセットする
        /// </summary>
        /// <param name="playerParkOpenDeckModel"></param>
        public void SetMainDeck (PlayerParkOpenDeckModel playerParkOpenDeckModel)
        {
            this.parkOpenSetMainDeckService.Execute(playerParkOpenDeckModel);
        }

        /// <summary>
        /// デッキにカードをセットする
        /// </summary>
        /// <param name="playerParkOpenDeckModel"></param>
        /// <param name="countType"></param>
        /// <param name="playerParkOpenCardModel"></param>
        public void SetCardToDeck(PlayerParkOpenDeckModel playerParkOpenDeckModel, PlayerParkOpenDeckModel.CountType countType, PlayerParkOpenCardModel playerParkOpenCardModel)
        {
            this.parkOpenSelectDeckCardService.Execute(playerParkOpenDeckModel, countType, playerParkOpenCardModel);
        }

        private IDisposable touchCardDisposable = null;

        /// <summary>
        /// カードの表示を行う
        /// </summary>
        public void PrepareParkOpen()
        {
            // メインデッキを表示
            var mainDeck = this.GetMainDeck();
            GameManager.Instance.GameUIManager.ParkOpenDeckPresenter.Show();
            GameManager.Instance.GameUIManager.ParkOpenDeckPresenter.SetContents(mainDeck);
            this.ResetPlayingCard();

            // カードをタップしたときのイベントを設定
            this.touchCardDisposable = GameManager.Instance.GameUIManager.ParkOpenDeckPresenter.OnTouchUseCardObservable.Subscribe(countType => {
                this.UsePlayingCard(countType);
            });

        }

        /// <summary>
        /// カード表示を終了する
        /// </summary>
        public void FinalizeParkOpen()
        {
            GameManager.Instance.GameUIManager.ParkOpenDeckPresenter.Close();

            if (this.touchCardDisposable != null)
            {
                this.touchCardDisposable.Dispose();
                this.touchCardDisposable = null;
            }
        }

        private void ResetPlayingCard()
        {            
            GameManager.Instance.GameUIManager.ParkOpenDeckPresenter.ResetCard();
            this.parkOpenUseCardService.ExecuteReset();
        }

        private void UsePlayingCard(PlayerParkOpenDeckModel.CountType countType)
        {
            GameManager.Instance.GameUIManager.ParkOpenDeckPresenter.UseCard(countType);
            this.parkOpenUseCardService.ExecuteCard(countType);
        }

        private PlayerParkOpenDeckModel GetMainDeck()
        {
            var currentDeck = this.playerParkOpenRepository.GetOwn().CurrentParkOpenDeckModel;
            Debug.Assert(currentDeck != null, "デッキがセットされていません。");
            return currentDeck;
        }
    }
}