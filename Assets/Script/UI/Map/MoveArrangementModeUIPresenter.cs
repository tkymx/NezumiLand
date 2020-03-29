using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class MoveArrangementModeUIPresenter : UiWindowPresenterBase
    {
        [SerializeField]
        private MoveArrangementModeUIView moveArrangementModeUIView = null;

        public void Initialize() {
            this.moveArrangementModeUIView.Initialize();
            this.disposables.Add(moveArrangementModeUIView.OnBackObservable.Subscribe(_ => {
                this.Close();
            }));
            this.Close();
        }
    }    
}