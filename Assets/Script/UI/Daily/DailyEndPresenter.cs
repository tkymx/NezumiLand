using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class DailyEndPresenter : UiWindowPresenterBase
    {
        [SerializeField]
        private DailyEndView dailyEndView = null;

        public void Initialize () {
            this.dailyEndView.Initialize();
            this.disposables.Add(this.dailyEndView.OnNextObservable.Subscribe(_ => {
                this.Close();
            }));
            this.Close();
        }

        public void SetCurrentChangeInfo (Currency currentCurrency, Currency nextCurrency, Satisfaction currentSatisfaction) {
            var deltaCurrency = nextCurrency - currentCurrency;
            this.dailyEndView.UpdateView(
                currentCurrency.ToString(),
                deltaCurrency.ToString(),
                nextCurrency.ToString(),
                currentSatisfaction.ToString(),
                DailyEarnCalculater.SatisfactionMultiRate.ToString(),
                "まあまあだね"
            );
        }
    }   
}