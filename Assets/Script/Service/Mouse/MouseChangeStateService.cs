using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class MouseChangeStateService 
    {
        private readonly IPlayerMouseViewRepository playerMouseViewRepository = null;

        public MouseChangeStateService(IPlayerMouseViewRepository playerMouseViewRepository)
        {
            this.playerMouseViewRepository = playerMouseViewRepository;
        }

        public void Execute(PlayerMouseViewModel playerMouseViewModel, MouseViewState state) {
            Debug.Assert(playerMouseViewModel != null, "PlayerMouseViewModelが nullです");
            playerMouseViewModel.ChangeState(state);
            this.playerMouseViewRepository.Store(playerMouseViewModel);
        }
    }    
}

