using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class MouseChangeTransformService 
    {
        private readonly IPlayerMouseViewRepository playerMouseViewRepository = null;

        public MouseChangeTransformService(IPlayerMouseViewRepository playerMouseViewRepository)
        {
            this.playerMouseViewRepository = playerMouseViewRepository;
        }

        public void Execute(PlayerMouseViewModel playerMouseViewModel, Vector3 position, Vector3 rotation) {
            Debug.Assert(playerMouseViewModel != null, "PlayerMouseViewModelが nullです");
            playerMouseViewModel.UpdateTransform(position, rotation);
            this.playerMouseViewRepository.Store(playerMouseViewModel);
        }
    }    
}

