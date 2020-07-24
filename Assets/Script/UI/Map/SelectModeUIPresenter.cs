using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class SelectModeUIPresenter : UiWindowPresenterBase
    {
        [SerializeField]
        private SelectModeUIView selectModeUIView = null;

        public void Initialize () {
            this.selectModeUIView.Initialize();
            this.Close();
        }
    }    
}
