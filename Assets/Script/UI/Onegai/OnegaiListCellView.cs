using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NL
{
    public class OnegaiListCellView : ListCellViewBase
    {
        [SerializeField]
        private Text titleText = null;

        [SerializeField]
        private Text detailText = null;

        public void UpdateCell(string title, string detail)
        {
            this.titleText.text = title;
            this.detailText.text = detail;
        }
    }
}
