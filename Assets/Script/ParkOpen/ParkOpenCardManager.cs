using System;
using UnityEngine;

namespace NL
{
    public class ParkOpenCardManager : Disposable
    {
        // レポジトリ
        private IPlayerParkOpenRepository playerParkOpenRepository = null;

        // 実行クラス
        private ParkOpenActionExecuter parkOpenActionExecuter = null;

        // サービス
        private ParkOpenCardCreateService parkOpenCardCreateService = null;
        private ParkOpenDeckCreateService parkOpenDeckCreateService = null;
        private ParkOpenSelectDeckCardService parkOpenSelectDeckCardService = null;
        private ParkOpenSetMainDeckService parkOpenSetMainDeckService = null;
        private ParkOpenUseCardService parkOpenUseCardService = null;

        /// <summary>
        /// カードの動作が終了を関し
        /// </summary>
        /// <value></value>
        public TypeObservable<ParkOpenCardModel> OnComplateCardUse { get; private set; }

        public ParkOpenCardManager (IPlayerParkOpenRepository playerParkOpenRepository, IPlayerParkOpenCardRepository playerParkOpenCardRepository, IPlayerParkOpenDeckRepository playerParkOpenDeckRepository)
        {
            this.playerParkOpenRepository = playerParkOpenRepository;
            this.parkOpenActionExecuter = new ParkOpenActionExecuter();

            this.OnComplateCardUse = new TypeObservable<ParkOpenCardModel>();

            this.parkOpenCardCreateService = new ParkOpenCardCreateService(playerParkOpenCardRepository);
            this.parkOpenDeckCreateService = new ParkOpenDeckCreateService(playerParkOpenDeckRepository);
            this.parkOpenSelectDeckCardService = new ParkOpenSelectDeckCardService(playerParkOpenDeckRepository);
            this.parkOpenSetMainDeckService = new ParkOpenSetMainDeckService(playerParkOpenRepository);
            this.parkOpenUseCardService = new ParkOpenUseCardService(playerParkOpenRepository);

            this.disposables.Add(this.parkOpenActionExecuter.OnComplate.Subscribe(parkOpenCardModel => {
                this.OnComplateCardUse.Execute(parkOpenCardModel);
            }));
        }

        public void UpdateByFrame()
        {
            this.parkOpenActionExecuter.UpdateByFrame();
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
            // アクションを実行する
            var currentDeck = this.playerParkOpenRepository.GetOwn().CurrentParkOpenDeckModel;
            var playerParkOpenCardModel = currentDeck.GetDeckCardModel(countType);
            this.parkOpenActionExecuter.Start(playerParkOpenCardModel.ParkOpenCardModel);

            // 見た目の変更を行う
            GameManager.Instance.GameUIManager.ParkOpenDeckPresenter.UseCard(countType);

            // データの変更
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