using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class PlayerMouseStockModel : ModelBase
    {
        public MouseOrderAmount MouseStockCount { get; private set; }

        public void Increment() {
            this.MouseStockCount += 1;
        }

        public PlayerMouseStockModel(
            uint id,
            long mouseStockCount
        )
        {
            this.Id = id;
            this.MouseStockCount = new MouseOrderAmount(mouseStockCount);            
        }
    }   
}