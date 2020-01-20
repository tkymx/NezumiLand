using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class EarnCurrencyAddService
    {
        private readonly IPlayerEarnCurrencyRepository playerEarnCurrencyRepository;

        public EarnCurrencyAddService(IPlayerEarnCurrencyRepository playerEarnCurrencyRepository)
        {
            this.playerEarnCurrencyRepository = playerEarnCurrencyRepository;            
        }

        public void Execute(PlayerEarnCurrencyModel playerEarnCurrencyModel, Currency additionalEarnCurrency)
        {
            playerEarnCurrencyModel.AddEarnCurrency(additionalEarnCurrency);
            playerEarnCurrencyRepository.Store(playerEarnCurrencyModel);
        }
    }   
}

