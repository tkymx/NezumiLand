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

        [SerializeField]
        private Button mousePurchaseButton = null;

        public void Inititialize() {
            this.mousePurchaseButton.onClick.AddListener(()=>{
                if (!GameManager.Instance.GameModeManager.IsMousePurchaseMode) {
                    GameManager.Instance.GameModeManager.EnqueueChangeModeWithHistory(GameModeGenerator.GenerateMousePurchaseMode());
                }
            });        
        }

        public void UpdateByFrame () {
            
            // 最大数
            this.mouseStockCountText.text = GameManager.Instance.MouseStockManager.MouseStockCount.ToString ();

            // 現在の数
            var currentCount = GameManager.Instance.MouseStockManager.CurrentWithReserve;
            this.orderedMouseCountText.text = currentCount.ToString ();
        }
    }
}