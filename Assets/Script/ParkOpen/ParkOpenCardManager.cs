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

        public ParkOpenCardManager (IPlayerParkOpenRepository playerParkOpenRepository, IPlayerParkOpenCardRepository playerParkOpenCardRepository, IPlayerParkOpenDeckRepository playerParkOpenDeckRepository)
        {
            this.playerParkOpenRepository = playerParkOpenRepository;

            this.parkOpenCardCreateService = new ParkOpenCardCreateService(playerParkOpenCardRepository);
            this.parkOpenDeckCreateService = new ParkOpenDeckCreateService(playerParkOpenDeckRepository);
            this.parkOpenSelectDeckCardService = new ParkOpenSelectDeckCardService(playerParkOpenDeckRepository);
            this.parkOpenSetMainDeckService = new ParkOpenSetMainDeckService(playerParkOpenRepository);
        }

        public void ObtainCard(ParkOpenCardModel parkOpenCardModel)
        {
            this.parkOpenCardCreateService.Execute(parkOpenCardModel);            
        }

        public void CreateDeck()
        {
            this.parkOpenDeckCreateService.Execute();
        }

        public void SetMainDeck (PlayerParkOpenDeckModel playerParkOpenDeckModel)
        {
            this.parkOpenSetMainDeckService.Execute(playerParkOpenDeckModel);
        }

        public void SetCardToDeck(PlayerParkOpenDeckModel playerParkOpenDeckModel, PlayerParkOpenDeckModel.CountType countType, PlayerParkOpenCardModel playerParkOpenCardModel)
        {
            this.parkOpenSelectDeckCardService.Execute(playerParkOpenDeckModel, countType, playerParkOpenCardModel);
        }

        public PlayerParkOpenDeckModel GetMainDeck()
        {
            var currentDeck = this.playerParkOpenRepository.GetOwn().CurrentParkOpenDeckModel;
            Debug.Assert(currentDeck != null, "デッキがセットされていません。");
            return currentDeck;
        }
    }
}