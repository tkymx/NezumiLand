using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace NL
{
    public class ImageView : MonoBehaviour
    {
        [SerializeField]
        private Button back;

        [SerializeField]
        private Image image;

        [SerializeField]
        private Text title;

        private TypeObservable<int> onBackObservable;
        public TypeObservable<int> OnBackObservable => onBackObservable;

        public void Initialize () {
            this.onBackObservable = new TypeObservable<int>();
            this.back.onClick.AddListener(()=>{
                this.onBackObservable.Execute(0);
            });
        }

        public void UpdateView (Sprite sprite, string title) {
            this.image.sprite = sprite;
            this.title.text = title;
        }
    }
   
}