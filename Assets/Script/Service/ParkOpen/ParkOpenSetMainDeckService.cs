using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    /// <summary>
    /// 
    /// </summary>
    public class ParkOpenSetMainDeckService 
    {
        private readonly IPlayerParkOpenRepository playerParkOpenRepository = null;

        public ParkOpenSetMainDeckService(IPlayerParkOpenRepository playerParkOpenRepository)
        {
            this.playerParkOpenRepository = playerParkOpenRepository;
        }

        public void Execute(PlayerParkOpenDeckModel playerParkOpenDeckModel) {
            var parkOpenModel = this.playerParkOpenRepository.GetOwn();
            parkOpenModel.SetParkOpenDeck(playerParkOpenDeckModel);
            this.playerParkOpenRepository.Store(parkOpenModel);
        }
    }    
}
