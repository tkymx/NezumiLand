using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    /// <summary>
    /// 配置するときのアイテムの管理を行う
    /// </summary>
    public class ArrangementResourceHelper
    {
        public static bool IsConsume(ArrangementResourceAmount amount)
        {
            if (!GameManager.Instance.Wallet.IsPay(amount.Currency))
            {
                return false;
            }

            if (!GameManager.Instance.ArrangementItemStore.IsConsume(amount.ArrangementItemAmount))
            {
                return false;
            }

            return true;
        }

        public static void Consume(ArrangementResourceAmount amount)
        {
            Debug.Assert(ArrangementResourceHelper.IsConsume(amount), "精算ができません");
            GameManager.Instance.Wallet.Pay(amount.Currency);
            GameManager.Instance.ArrangementItemStore.Consume(amount.ArrangementItemAmount);
        }

        public static void Push(ArrangementResourceAmount amount)
        {
            GameManager.Instance.Wallet.Push(amount.Currency);
            GameManager.Instance.ArrangementItemStore.Push(amount.ArrangementItemAmount);
        }
    }
}

