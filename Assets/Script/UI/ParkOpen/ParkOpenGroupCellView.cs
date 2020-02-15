using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NL
{
    public class ParkOpenGroupCellView : MonoBehaviour
    {
        [SerializeField]
        private Image frameImage = null;

        [SerializeField]
        private Image iconImage = null;

        [SerializeField]
        private GameObject selectIndicator = null;

        [SerializeField]
        private GameObject clearIndicator = null;

        [SerializeField]
        private Button selectButton = null;

        public TypeObservable<int> OnSelectObservable { get; private set; }

        public void Initialize() {
            this.OnSelectObservable = new TypeObservable<int>();
            this.selectButton.onClick.AddListener(()=>{
                this.OnSelectObservable.Execute(0);
            });
        }

        public void UpdateView(Sprite frameImage, Sprite iconImage, bool isClear) {
            this.frameImage.sprite = frameImage;
            this.iconImage.sprite = iconImage;
            this.clearIndicator.SetActive(isClear);
        }

        public void SetSelectVisible(bool isSelect)
        {
            this.selectIndicator.SetActive(isSelect);
        }
    }   
}