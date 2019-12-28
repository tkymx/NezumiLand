using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class InitializePlayerInfoService
    {
        private readonly IPlayerInfoRepository playerInfoRepository = null;

        public InitializePlayerInfoService(IPlayerInfoRepository playerInfoRepository) {
            this.playerInfoRepository = playerInfoRepository;
        }

        public void Execute () {
            var playerInfoModel = this.playerInfoRepository.GetOwn();
            GameManager.Instance.TimeManager.ForceSet(playerInfoModel.ElapsedTime);
            GameManager.Instance.Wallet.ForceSet(playerInfoModel.Currency);
            GameManager.Instance.ArrangementItemStore.ForceSet(playerInfoModel.ArrangementItemAmount);
        }
    }   
}
