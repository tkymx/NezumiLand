using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class PlayerEarnCurrencyModel : ModelBase
    {
        public Currency EarnCurrency { get; private set; }
        public PlayerArrangementTargetModel PlayerArrangementTargetModel { get; private set; }

        public PlayerEarnCurrencyModel(
            uint id,
            Currency earnCurrency,
            PlayerArrangementTargetModel playerArrangementTargetModel
        )
        {
            this.Id = id;
            this.EarnCurrency = earnCurrency;
            this.PlayerArrangementTargetModel = playerArrangementTargetModel;
        }

        public void AddEarnCurrency (Currency additionalEarnCurrency) {
            this.EarnCurrency += additionalEarnCurrency;
        } 
    }   
}