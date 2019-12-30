using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class ImagePresenter : UiWindowPresenterBase
    {
        [SerializeField]
        private ImageView imageView = null;

        public void Initialize () {
            this.imageView.Initialize();
            this.disposables.Add(this.imageView.OnBackObservable.Subscribe(_ => {
                this.Close();
            }));
            this.Close();
        }

        public void SetImage (Sprite sprite, string title) {
            this.imageView.UpdateView(sprite, title);
        }
    }   
}