using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class ParkOpenGroupManager
    {
        private IParkOpenGroupRepository parkOpenGroupRepository;
        private IPlayerParkOpenGroupRepository playerParkOpenGroupRepository;
        private ParkOpenGroupClearService parkOpenGroupClearService;
        private ParkOpenGroupSetGroupOpenTypeService parkOpenGroupSetGroupOpenTypeService;
        
        public ParkOpenGroupManager(IParkOpenGroupRepository parkOpenGroupRepository, IPlayerParkOpenGroupRepository playerParkOpenGroupRepository)
        {
            this.parkOpenGroupRepository = parkOpenGroupRepository;
            this.playerParkOpenGroupRepository = playerParkOpenGroupRepository;
            this.parkOpenGroupClearService = new ParkOpenGroupClearService(playerParkOpenGroupRepository);
            this.parkOpenGroupSetGroupOpenTypeService = new ParkOpenGroupSetGroupOpenTypeService(parkOpenGroupRepository, playerParkOpenGroupRepository);
        }

        public void ClearGroup(ParkOpenGroupModel parkOpenGroupModel)
        {
            this.parkOpenGroupClearService.Execute(parkOpenGroupModel);
        }

        public void LotParkOpenGroup()
        {
            this.parkOpenGroupSetGroupOpenTypeService.Execute();
        }

    }
    
}
