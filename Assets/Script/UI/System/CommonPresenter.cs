using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class CommonPresenter : UiWindowPresenterBase
    {
        [SerializeField]
        private CommonView commonView = null;

        public void Initialize() {
            this.commonView.Initialize();
            this.disposables.Add(commonView.OnBackObservable.Subscribe(_ => {
                this.Close();
            }));
            this.Close();
        }

        public void SetContents(string title, string contents) {
            this.commonView.UpdateView(title, contents);            
        }
    }    
}