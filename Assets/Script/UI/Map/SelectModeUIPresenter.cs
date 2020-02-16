using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class SelectModeUIPresenter : UiWindowPresenterBase
    {
        [SerializeField]
        private SelectModeUIView selectModeUIView = null;

        public TypeObservable<int> OnClickParkOpenSelectObservable { get; private set; }

        public void Initialize () {
            this.OnClickParkOpenSelectObservable = new TypeObservable<int>();
            this.selectModeUIView.Initialize();
            this.disposables.Add(this.selectModeUIView.OnClickParkOpenSelectObservable.Subscribe(_ => {
                this.OnClickParkOpenSelectObservable.Execute(_);
            }));
            this.Close();
        }
    }    
}
