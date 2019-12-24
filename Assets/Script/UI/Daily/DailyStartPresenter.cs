using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class DailyStartPresenter : UiWindowPresenterBase
    {
        [SerializeField]
        DailyStartView dailyStartView = null;

        public void Initialize() {
            this.dailyStartView.Initialize();
            this.disposables.Add(this.dailyStartView.OnNextObservable.Subscribe(_ => {
                this.Close();
            }));
            this.Close();
        }

        public override void onPrepareShow() {
            this.dailyStartView.UpdateView("「" + GameManager.Instance.TimeManager.ToString()+"」の始まり");
        }
    }   
}