using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class EarnCurrencyRemoveService
    {
        private readonly IPlayerEarnCurrencyRepository playerEarnCurrencyRepository;

        public EarnCurrencyRemoveService(IPlayerEarnCurrencyRepository playerEarnCurrencyRepository)
        {
            this.playerEarnCurrencyRepository = playerEarnCurrencyRepository;            
        }

        public void Execute(PlayerEarnCurrencyModel playerEarnCurrencyModel)
        {
            playerEarnCurrencyRepository.Remove(playerEarnCurrencyModel);
        }
    }   
}

