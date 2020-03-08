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
        private ParkOpenGroupOpenService parkOpenGroupOpenService;
        
        public ParkOpenGroupManager(IParkOpenGroupRepository parkOpenGroupRepository, IPlayerParkOpenGroupRepository playerParkOpenGroupRepository)
        {
            this.parkOpenGroupRepository = parkOpenGroupRepository;
            this.playerParkOpenGroupRepository = playerParkOpenGroupRepository;
            this.parkOpenGroupClearService = new ParkOpenGroupClearService(playerParkOpenGroupRepository);
            this.parkOpenGroupSetGroupOpenTypeService = new ParkOpenGroupSetGroupOpenTypeService(parkOpenGroupRepository, playerParkOpenGroupRepository);
            this.parkOpenGroupOpenService = new ParkOpenGroupOpenService(playerParkOpenGroupRepository);
        }

        public void ToClearGroup(ParkOpenGroupModel parkOpenGroupModel)
        {
            this.parkOpenGroupClearService.Execute(parkOpenGroupModel);
        }

        public void LotParkOpenGroup()
        {
            this.parkOpenGroupSetGroupOpenTypeService.Execute();
        }

        public void ToOpen(ParkOpenGroupModel parkOpenGroupModel)
        {
            this.parkOpenGroupOpenService.Execute(parkOpenGroupModel);
        }

        public void ToOpen(uint parkOpenGroupModelId)
        {
            this.parkOpenGroupOpenService.Execute(parkOpenGroupModelId);
        }
    }
    
}
