using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public class ReserveArrangementService
    {
        private readonly IPlayerArrangementTargetRepository playerArrangementTargetRepository = null;
        
        public ReserveArrangementService(IPlayerArrangementTargetRepository playerArrangementTargetRepository) 
        {
            this.playerArrangementTargetRepository = playerArrangementTargetRepository;
        }
        public void Execute (IPlayerArrangementTarget arrangementTarget, MonoInfo monoInfo) {

            // プレイヤー情報の変更
            arrangementTarget.RegisterMaking(monoInfo);
            playerArrangementTargetRepository.Store(arrangementTarget.PlayerArrangementTargetModel);

            // 予約
            ArrangementResourceHelper.ReserveConsume(arrangementTarget.MonoInfo.ArrangementResourceAmount);
            GameManager.Instance.ArrangementManager.AddArrangement(arrangementTarget);
        }
    }
}

