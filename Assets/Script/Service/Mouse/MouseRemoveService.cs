using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class MouseRemoveService 
    {
        private readonly IPlayerMouseViewRepository playerMouseViewRepository = null;

        public MouseRemoveService(IPlayerMouseViewRepository playerMouseViewRepository)
        {
            this.playerMouseViewRepository = playerMouseViewRepository;
        }

        public void Execute(PlayerMouseViewModel playerMouseViewModel) {
            Debug.Assert(playerMouseViewModel != null, "PlayerMouseViewModelが nullです");
            this.playerMouseViewRepository.Remove(playerMouseViewModel);
        }
    }    
}

