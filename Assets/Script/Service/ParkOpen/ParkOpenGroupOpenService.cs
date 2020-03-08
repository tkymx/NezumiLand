using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    /// <summary>
    /// 
    /// </summary>
    public class ParkOpenGroupOpenService 
    {
        private readonly IPlayerParkOpenGroupRepository playerParkOpenGroupRepository = null;

        public ParkOpenGroupOpenService(IPlayerParkOpenGroupRepository playerParkOpenGroupRepository)
        {
            this.playerParkOpenGroupRepository = playerParkOpenGroupRepository;
        }

        public void Execute(ParkOpenGroupModel parkOpenGroupModel) {
            this.Execute(parkOpenGroupModel.Id);
        }

        public void Execute(uint parkOpenGroupModelId) {
            var playerParkOpenGroupModel = this.playerParkOpenGroupRepository.Get(parkOpenGroupModelId);
            playerParkOpenGroupModel.ToOpen();
            this.playerParkOpenGroupRepository.Store(playerParkOpenGroupModel);
        }        
    }    
}
