using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class UpdateTimeService
    {
        private readonly IPlayerInfoRepository playerInfoRepository = null;

        public UpdateTimeService(IPlayerInfoRepository playerInfoRepository) {
            this.playerInfoRepository = playerInfoRepository;
        }

        public void Execute (float time) {
            var playerInfoModel = this.playerInfoRepository.GetOwn();
            playerInfoModel.UpdateTime(time);
            this.playerInfoRepository.Store(playerInfoModel);
        }
    }   
}

