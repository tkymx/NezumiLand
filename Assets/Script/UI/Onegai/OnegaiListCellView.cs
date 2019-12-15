using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NL {
    public class OnegaiListCellView : ListCellViewBase {
        [SerializeField]
        private Text titleText = null;

        [SerializeField]
        private Text detailText = null;

        [SerializeField]
        private OnegaiCloseTimeView closeTimeView = null;

        [SerializeField]
        private GameObject clearBadge = null;

        // 終了までの時間
        private float closeTime = 0;

        public void UpdateCell (string title, string detail, bool isClear, bool isShowClose,float closeTime) {
            this.titleText.text = title;
            this.detailText.text = detail;
            this.clearBadge.SetActive (isClear);
            this.closeTimeView.UpdateView(isShowClose, closeTime);
        }
    }
}