using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NL
{
    public class DailyEndView : MonoBehaviour
    {
        [SerializeField]
        private Text currentCurrency = null;

        [SerializeField]
        private Text deltaCurrency = null;

        [SerializeField]
        private Text nextCurrency = null;

        [SerializeField]
        private Text currentAllSatisfaction = null;

        [SerializeField]
        private Text satisfactionMultiRate = null;

        [SerializeField]
        private Text evaluation = null;

        [SerializeField]
        private Button toNext = null;

        private TypeObservable<int> onNextObservable = null;
        public TypeObservable<int> OnNextObservable => onNextObservable;

        public void Initialize () {
            this.onNextObservable = new TypeObservable<int>();
            this.toNext.onClick.AddListener(()=>{
                this.onNextObservable.Execute(0);
            });
        }

        public void UpdateView (string currentCurrency,string deltaCurrency, string nextCurrency, string currentAllSatisfaction,string satisfactionMultiRate, string evaluation) {
            this.deltaCurrency.text = deltaCurrency;
            this.currentAllSatisfaction.text = currentAllSatisfaction;
            this.currentCurrency.text = currentCurrency;
            this.nextCurrency.text = nextCurrency;
            this.satisfactionMultiRate.text = satisfactionMultiRate;
            this.evaluation.text = evaluation;
        }

    }   
}