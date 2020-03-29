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
            arrangementTarget.SetPosition(positions);

            // MonoView 位置も変更
            if (arrangementTarget.HasMonoViewModel)
            {
                arrangementTarget.MonoViewModel.SetPosition(arrangementTarget.CenterPosition);
            }

            // Playerデータを持っている場合は保存
            if (arrangementTarget.PlayerArrangementTargetModel != null)
            {
                playerArrangementTargetRepository.Store(arrangementTarget.PlayerArrangementTargetModel);
            }
        }
    }
}

