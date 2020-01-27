using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    /// <summary>
    /// 
    /// </summary>
    public class ParkOpenObtainHeartService 
    {
        private readonly IPlayerParkOpenRepository playerParkOpenRepository = null;

        public ParkOpenObtainHeartService(IPlayerParkOpenRepository playerParkOpenRepository)
        {
            this.playerParkOpenRepository = playerParkOpenRepository;
        }

        public void Execute(int increaseCount = 1) {
            var playerParkOpenModel = this.playerParkOpenRepository.GetOwn();
            playerParkOpenModel.AddHeartCount(increaseCount);
            this.playerParkOpenRepository.Store(playerParkOpenModel);
        }
    }    
}

