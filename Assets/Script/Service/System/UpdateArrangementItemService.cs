using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class UpdateArrangementItemService
    {
        private readonly IPlayerInfoRepository playerInfoRepository = null;

        public UpdateArrangementItemService(IPlayerInfoRepository playerInfoRepository) {
            this.playerInfoRepository = playerInfoRepository;
        }

        public void Execute (ArrangementItemAmount arrangementItemAmount) {
            var playerInfoModel = this.playerInfoRepository.GetOwn();
            playerInfoModel.UpdateArrangementItemAmount(arrangementItemAmount);
            this.playerInfoRepository.Store(playerInfoModel);
        }
    }   
}
