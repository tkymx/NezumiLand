using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    /// <summary>
    /// デッキにカードをセットする
    /// </summary>
    public class ParkOpenSelectDeckCardService 
    {
        private readonly IPlayerParkOpenDeckRepository playerParkOpenDeckRepository = null;

        public ParkOpenSelectDeckCardService(IPlayerParkOpenDeckRepository playerParkOpenDeckRepository)
        {
            this.playerParkOpenDeckRepository = playerParkOpenDeckRepository;
        }

        public void Execute(PlayerParkOpenDeckModel playerParkOpenDeckModel, PlayerParkOpenDeckModel.CountType countType, PlayerParkOpenCardModel playerParkOpenCardModel) {
            playerParkOpenDeckModel.SetCard(countType, playerParkOpenCardModel);
            this.playerParkOpenDeckRepository.Store(playerParkOpenDeckModel);
        }
    }    
}
