using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class MonoViewRemoveService
    {
        private readonly IPlayerMonoViewRepository playerMonoViewRepository = null;

        public MonoViewRemoveService(IPlayerMonoViewRepository playerMonoViewRepository) {
            this.playerMonoViewRepository = playerMonoViewRepository;
        }

        public void Execute(MonoViewModel monoViewModel) {
            Debug.Assert(monoViewModel.PlayerMonoViewModel != null, "MonoViewRemoveServiceが開始できません");
            this.playerMonoViewRepository.Remove(monoViewModel.PlayerMonoViewModel);
        }
    }   
}