using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NL {
    public class MouseStockCountPresenter : MonoBehaviour {
        [SerializeField]
        private Text mouseStockCountText = null;

        [SerializeField]
        private Text orderedMouseCountText = null;

        void Update () {
            this.mouseStockCountText.text = GameManager.Instance.MouseStockManager.MouseStockCount.ToString ();
            this.orderedMouseCountText.text = GameManager.Instance.MouseStockManager.OrderedMouseCount.ToString ();
        }
    }
}