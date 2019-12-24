using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class MonoLevelUpServive
    {
        private readonly IPlayerMonoViewRepository playerMonoViewRepository = null;

        public MonoLevelUpServive(IPlayerMonoViewRepository playerMonoViewRepository) {
            this.playerMonoViewRepository = playerMonoViewRepository;
        }

        public void Execute(PlayerMonoViewModel playerMonoViewModel) {
            playerMonoViewModel.LevelUp();
            playerMonoViewRepository.Store(playerMonoViewModel);
        }
    }   
}