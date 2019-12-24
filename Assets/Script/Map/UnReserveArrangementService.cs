using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public class UnReserveArrangementService
    {
        private readonly IPlayerArrangementTargetRepository playerArrangementTargetRepository = null;

        public UnReserveArrangementService(IPlayerArrangementTargetRepository playerArrangementTargetRepository)
        {
            this.playerArrangementTargetRepository = playerArrangementTargetRepository;            
        }

        public void Execute (IPlayerArrangementTarget arrangementTarget) {

            // 予約状況からも削除
            ArrangementResourceHelper.UnReserveConsume(arrangementTarget.MonoInfo.ArrangementResourceAmount);
            GameManager.Instance.ArrangementManager.RemoveArranement(arrangementTarget);
        }
    }
}

