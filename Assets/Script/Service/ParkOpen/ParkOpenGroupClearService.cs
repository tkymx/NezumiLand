using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    /// <summary>
    /// 
    /// </summary>
    public class ParkOpenGroupClearService 
    {
        private readonly IPlayerParkOpenGroupRepository playerParkOpenGroupRepository = null;

        public ParkOpenGroupClearService(IPlayerParkOpenGroupRepository playerParkOpenGroupRepository)
        {
            this.playerParkOpenGroupRepository = playerParkOpenGroupRepository;
        }

        public void Execute(ParkOpenGroupModel parkOpenGroupModel) {
            var playerParkOpenGroupModel = this.playerParkOpenGroupRepository.Get(parkOpenGroupModel.Id);
            playerParkOpenGroupModel.ToClear();
            this.playerParkOpenGroupRepository.Store(playerParkOpenGroupModel);
        }
    }    
}
