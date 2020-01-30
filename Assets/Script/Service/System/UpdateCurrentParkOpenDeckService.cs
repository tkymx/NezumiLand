using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class UpdateCurrentParkOpenDeckService
    {
        private readonly IPlayerInfoRepository playerInfoRepository = null;

        public UpdateCurrentParkOpenDeckService(IPlayerInfoRepository playerInfoRepository) {
            this.playerInfoRepository = playerInfoRepository;
        }

        public void Execute (PlayerParkOpenDeckModel playerParkOpenDeckModel) {
            var playerInfoModel = this.playerInfoRepository.GetOwn();
            playerInfoModel.SetParkOpenDeck(playerParkOpenDeckModel);
            this.playerInfoRepository.Store(playerInfoModel);
        }
    }   
}
