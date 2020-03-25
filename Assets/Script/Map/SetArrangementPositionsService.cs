using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public class SetArrangementPositionsService
    {
        private readonly IPlayerArrangementTargetRepository playerArrangementTargetRepository = null;
        
        public SetArrangementPositionsService(IPlayerArrangementTargetRepository playerArrangementTargetRepository) 
        {
            this.playerArrangementTargetRepository = playerArrangementTargetRepository;
        }
        public void Execute (IPlayerArrangementTarget arrangementTarget, List<ArrangementPosition> positions) {

            // プレイヤーの座標を変更
            arrangementTarget.PlayerArrangementTargetModel.SetPosition(positions);
            playerArrangementTargetRepository.Store(arrangementTarget.PlayerArrangementTargetModel);
        }
    }
}

