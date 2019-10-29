using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NL
{
    public class WalletPresenter : MonoBehaviour
    {
        [SerializeField]
        Text text = null;

        // Update is called once per frame
        void Update()
        {
            text.text = GameManager.Instance.Wallet.CurrentCurrency.Value.ToString();
        }
    }
}

