using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class MouseChangeMakingAmountService 
    {
        private readonly IPlayerMouseViewRepository playerMouseViewRepository = null;

        public MouseChangeMakingAmountService(IPlayerMouseViewRepository playerMouseViewRepository)
        {
            this.playerMouseViewRepository = playerMouseViewRepository;
        }

        public void Execute(PlayerMouseViewModel playerMouseViewModel, MakingAmount makingAmount) {
            Debug.Assert(playerMouseViewModel != null, "PlayerMouseViewModelが nullです");
            playerMouseViewModel.UpdateMakingAmount(makingAmount);
            this.playerMouseViewRepository.Store(playerMouseViewModel);
        }
    }    
}

