using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NL {
    public class WalletPresenter : MonoBehaviour {
        [SerializeField]
        Text text = null;

        private void Update () {
            text.text = GameManager.Instance.Wallet.CurrentWithReserve.Value.ToString ();
        }
    }
}