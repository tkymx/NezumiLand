using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class PlayerMouseStockModel : ModelBase
    {
        public int MouseStockCount { get; private set; }

        public void Increment() {
            this.MouseStockCount++;
        }

        public PlayerMouseStockModel(
            uint id,
            int mouseStockCount
        )
        {
            this.Id = id;
            this.MouseStockCount = mouseStockCount;            
        }
    }   
}