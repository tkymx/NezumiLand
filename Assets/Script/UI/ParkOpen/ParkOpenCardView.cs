using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NL
{
    public class ParkOpenCardView : MonoBehaviour
    {
        [SerializeField]
        private Text nameText = null;

        [SerializeField]
        private Text descriptionText = null;

        [SerializeField]
        private Image image = null;

        [SerializeField]
        private Button cardButton = null;

        public TypeObservable<int> OnTouchCardObservable { get; private set; }

        public void Initialize() {
            this.OnTouchCardObservable = new TypeObservable<int>();
            this.cardButton.onClick.AddListener(()=>{
                this.OnTouchCardObservable.Execute(0);
            });
        }

        public void UpdateView(string name, string description, Sprite sprite) {
            this.nameText.text = name;
            this.descriptionText.text = description;
            this.image.sprite = sprite;
        }
    }   
}