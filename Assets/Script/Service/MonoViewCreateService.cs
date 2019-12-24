using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class MonoViewCreateService
    {
        private readonly IPlayerMonoViewRepository playerMonoViewRepository = null;

        public MonoViewCreateService(IPlayerMonoViewRepository playerMonoViewRepository) {
            this.playerMonoViewRepository = playerMonoViewRepository;
        }

        public PlayerMonoViewModel Execute(uint monoId) {
            var playerMonoViewModel = this.playerMonoViewRepository.Create(monoId, 1);
            return playerMonoViewModel;
        }
    }   
}