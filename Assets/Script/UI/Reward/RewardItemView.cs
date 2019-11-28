using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NL {
    public class RewardItemView : MonoBehaviour
    {
        [SerializeField]
        private Text rewardItemName = null;

        [SerializeField]
        private Text rewardItemAmount = null;

        [SerializeField]
        private Image rewardItemImage = null;

        [SerializeField]
        private Button nextButton = null;

        /// <summary>
        ///  クリックしたときに実装
        /// </summary>
        private TypeObservable<int> onClickObservable;
        public TypeObservable<int> OnClickObservable => onClickObservable;

        public void Initialize() {
            this.onClickObservable = new TypeObservable<int>();
            this.nextButton.onClick.AddListener(() => {
                this.onClickObservable.Execute(0);
            });
        }

        public void UpdateReward(string name, uint amount, Sprite sprite) {
            this.rewardItemName.text = name;
            this.rewardItemAmount.text = "x" + amount.ToString();
            this.rewardItemImage.sprite = sprite;
        }
    }
}