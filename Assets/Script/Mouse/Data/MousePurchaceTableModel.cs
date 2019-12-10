using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    /// <summary>
    /// マウス購入時のコストを保持、IDは購入するネズミの次の数（○個目）
    /// </summary>
    public class MousePurchaceTableModel : ModelBase
    {
        public Currency CurrencyCost { get; private set; }
        public ArrangementItemAmount ItemConst { get; private set; }

        public MousePurchaceTableModel(
            uint id,
            long currencyCost,
            long itemCost
        )
        {
            this.Id = id;
            this.CurrencyCost = new Currency(currencyCost);
            this.ItemConst = new ArrangementItemAmount(itemCost);            
        }

        public MousePurchaseResourceAmount GetMousePurchaseResourceAmount() {
            return new MousePurchaseResourceAmount(CurrencyCost, ItemConst);
        }
    }
}

