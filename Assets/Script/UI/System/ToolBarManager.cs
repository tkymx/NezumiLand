using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public class ToolBarManager : MonoBehaviour {

        [SerializeField]
        private MouseStockCountPresenter mouseStockCountPresenter = null;

        public void Initialize () {
            this.mouseStockCountPresenter.Inititialize();
        }

        public void UpdateByrame () {
            this.mouseStockCountPresenter.UpdateByFrame();
        }
    }
}