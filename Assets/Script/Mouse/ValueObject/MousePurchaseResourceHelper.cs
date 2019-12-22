using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    /// <summary>
    /// マウスを購入するときの資源の管理を行う
    /// </summary>
    public class MousePurchaseResourceHelper {
        public static bool IsConsume (MousePurchaseResourceAmount amount) {
            if (!GameManager.Instance.Wallet.IsConsume (amount.Currency)) {
                return false;
            }

            if (!GameManager.Instance.ArrangementItemStore.IsConsume (amount.ArrangementItemAmount)) {
                return false;
            }

            return true;
        }

        public static void Consume (MousePurchaseResourceAmount amount) {
            Debug.Assert (MousePurchaseResourceHelper.IsConsume (amount), "精算ができません");
            GameManager.Instance.Wallet.Consume (amount.Currency);
            GameManager.Instance.ArrangementItemStore.Consume (amount.ArrangementItemAmount);
        }
    }
}