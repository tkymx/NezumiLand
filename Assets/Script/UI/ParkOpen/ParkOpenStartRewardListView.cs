using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NL {
    public class ParkOpenStartRewardListView : ListCellViewBase {
        [SerializeField]
        private Text itemName = null;

        [SerializeField]
        private Image itemIcon = null;

        [SerializeField]
        private Text itemCount = null;

        public override void Initialize() {
            base.Initialize();
        }

        public void UpdateCell (string name, Sprite icon, string count) {
            this.itemName.text = name;
            this.itemIcon.sprite = icon;
            this.itemCount.text = count;
        }
    }
}