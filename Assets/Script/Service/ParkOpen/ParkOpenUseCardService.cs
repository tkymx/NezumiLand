using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    /// <summary>
    /// 
    /// </summary>
    public class ParkOpenUseCardService 
    {
        private readonly IPlayerParkOpenRepository playerParkOpenRepository = null;

        public ParkOpenUseCardService(IPlayerParkOpenRepository playerParkOpenRepository)
        {
            this.playerParkOpenRepository = playerParkOpenRepository;
        }

        public void ExecuteReset() {
            var playerParkOpenModel = this.playerParkOpenRepository.GetOwn();
            playerParkOpenModel.ResetCardUse();
            this.playerParkOpenRepository.Store(playerParkOpenModel);
        }

        public void ExecuteCard(PlayerParkOpenDeckModel.CountType countType) {
            var playerParkOpenModel = this.playerParkOpenRepository.GetOwn();
            playerParkOpenModel.UseCard(countType);
            this.playerParkOpenRepository.Store(playerParkOpenModel);
        }
    }    
}
