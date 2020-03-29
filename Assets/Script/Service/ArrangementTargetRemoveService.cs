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

        public void Execute (PlayerArrangementTargetModel playerArrangementTargetModel) {
            Debug.Assert(playerArrangementTargetModel !=null, "ArrangementTargetRemoveServiceが実行できません");
            playerArrangementTargetRepository.Remove(playerArrangementTargetModel);
        }
    }   
}