using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    /// <summary>
    /// 
    /// </summary>
    public class ParkOpenGroupSetGroupOpenTypeService 
    {
        private readonly IParkOpenGroupRepository parkOpenGroupRepository = null;
        private readonly IPlayerParkOpenGroupRepository playerParkOpenGroupRepository = null;

        public ParkOpenGroupSetGroupOpenTypeService(IParkOpenGroupRepository parkOpenGroupRepository, IPlayerParkOpenGroupRepository playerParkOpenGroupRepository)
        {
            this.parkOpenGroupRepository = parkOpenGroupRepository;
            this.playerParkOpenGroupRepository = playerParkOpenGroupRepository;
        }

        public void Execute() {

            var dirtyList = new List<PlayerParkOpenGroupModel>();
            foreach (var parkOpenGroup in this.parkOpenGroupRepository.GetAll())
            {
                var playerParkOpenGroupModel = this.playerParkOpenGroupRepository.Get(parkOpenGroup.Id);
                playerParkOpenGroupModel.Lot();
                dirtyList.Add(playerParkOpenGroupModel);
            }
            this.playerParkOpenGroupRepository.Store(dirtyList);
        }
    }    
}
