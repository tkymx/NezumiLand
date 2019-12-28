using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NL {
    public class OnegaiListCellView : ListCellViewBase {
        [SerializeField]
        private Text titleText = null;

        [SerializeField]
        private OnegaiCloseTimeView closeTimeView = null;

        [SerializeField]
        private GameObject clearBadge = null;

        [SerializeField]
        private Text satisfaction = null;

        // 終了までの時間
        private float closeTime = 0;

        public void UpdateCell (string title, bool isClear, bool isShowClose,float closeTime, string satisfaction) {
            this.titleText.text = title;
            this.clearBadge.SetActive (isClear);
            this.closeTimeView.UpdateView(isShowClose, closeTime);
            this.satisfaction.text = "取得満足度: " + satisfaction;
        }
    }
}