using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class UpdateCurrencyService
    {
        private readonly IPlayerInfoRepository playerInfoRepository = null;

        public UpdateCurrencyService(IPlayerInfoRepository playerInfoRepository) {
            this.playerInfoRepository = playerInfoRepository;
        }

        public void Execute (Currency currency) {
            var playerInfoModel = this.playerInfoRepository.GetOwn();
            playerInfoModel.UpdateCurrency(currency);
            this.playerInfoRepository.Store(playerInfoModel);
        }
    }   
}
