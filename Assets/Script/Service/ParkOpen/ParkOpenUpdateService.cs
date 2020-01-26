using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class ParkOpenUpdateService 
    {
        private readonly IPlayerParkOpenRepository playerParkOpenRepository = null;

        public ParkOpenUpdateService(IPlayerParkOpenRepository playerParkOpenRepository)
        {
            this.playerParkOpenRepository = playerParkOpenRepository;
        }

        public void Execute(bool isOpen, float elapsedTime = 0, int nextWave = 0, ParkOpenGroupModel parkOpenGroupModel = null) {
            var playerParkOpenModel = this.playerParkOpenRepository.GetOwn();
            playerParkOpenModel.Update(
                isOpen,
                elapsedTime,
                nextWave,
                parkOpenGroupModel
            );
            this.playerParkOpenRepository.Store(playerParkOpenModel);
        }
    }    
}

