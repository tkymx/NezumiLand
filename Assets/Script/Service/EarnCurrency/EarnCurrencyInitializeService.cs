using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class EarnCurrencyInitializeService
    {
        private readonly IPlayerEarnCurrencyRepository playerEarnCurrencyRepository;

        public EarnCurrencyInitializeService(IPlayerEarnCurrencyRepository playerEarnCurrencyRepository)
        {
            this.playerEarnCurrencyRepository = playerEarnCurrencyRepository;          
        }

        public void Execute() {

            // Viewの追加
            foreach (var playerEarnCurrencyModel in this.playerEarnCurrencyRepository.GetAll())
            {
                GameManager.Instance.EarnCurrencyManager.CreateFromEarnCurrencyModel(playerEarnCurrencyModel);
            }
        }
    }   
}
