using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class MouseCreateService 
    {
        private readonly IPlayerMouseViewRepository playerMouseViewRepository = null;

        public MouseCreateService(IPlayerMouseViewRepository playerMouseViewRepository)
        {
            this.playerMouseViewRepository = playerMouseViewRepository;
        }

        public PlayerMouseViewModel Execute(Vector3 initialPosition, PlayerArrangementTargetModel playerArrangementTargetModel) {
            return this.playerMouseViewRepository.Create(initialPosition, new MakingAmount(0,playerArrangementTargetModel.MonoInfo.MakingTime), playerArrangementTargetModel);
        }
    }    
}

