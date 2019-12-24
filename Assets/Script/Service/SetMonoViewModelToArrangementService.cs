using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class SetMonoViewModelToArrangementService
    {
        private readonly IPlayerArrangementTargetRepository playerArrangementTargetRepository = null;

        public SetMonoViewModelToArrangementService(IPlayerArrangementTargetRepository playerArrangementTargetRepository)
        {
            this.playerArrangementTargetRepository = playerArrangementTargetRepository;
        }

        public void Execute(IPlayerArrangementTarget playerArrangementTarget, MonoViewModel monoViewModel) {
            playerArrangementTarget.RegisterMade(monoViewModel);
            this.playerArrangementTargetRepository.Store(playerArrangementTarget.PlayerArrangementTargetModel);
        }
    }    
}