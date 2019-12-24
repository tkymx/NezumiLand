using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class ArrangementTargetRemoveService
    {
        private readonly IPlayerArrangementTargetRepository playerArrangementTargetRepository = null;

        public ArrangementTargetRemoveService(IPlayerArrangementTargetRepository playerArrangementTargetRepository) {
            this.playerArrangementTargetRepository = playerArrangementTargetRepository;
        }

        public void Execute (IPlayerArrangementTarget arrangementTarget) {
            Debug.Assert(arrangementTarget.PlayerArrangementTargetModel!=null, "ArrangementTargetRemoveServiceが実行できません");
            playerArrangementTargetRepository.Remove(arrangementTarget.PlayerArrangementTargetModel);
        }
    }   
}