using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class EarnCurrencyCreateService
    {
        private readonly IPlayerEarnCurrencyRepository playerEarnCurrencyRepository;

        public EarnCurrencyCreateService(IPlayerEarnCurrencyRepository playerEarnCurrencyRepository)
        {
            this.playerEarnCurrencyRepository = playerEarnCurrencyRepository;            
        }

        public PlayerEarnCurrencyModel Execute(PlayerArrangementTargetModel playerArrangementTargetModel, Currency earnCurrency)
        {
            return playerEarnCurrencyRepository.Create(playerArrangementTargetModel, earnCurrency);
        }
    }   
}

