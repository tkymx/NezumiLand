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
        private GameObject clearBadge = null;

        public void UpdateCell (string title, string detail, bool isClear) {
            this.titleText.text = title;
            this.detailText.text = detail;
            this.clearBadge.SetActive (isClear);
        }
    }
}