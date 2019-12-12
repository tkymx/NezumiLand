using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NL
{
    public class DailyStartView : MonoBehaviour
    {
        [SerializeField]
        private Button onNext;

        private TypeObservable<int> onNextObservable = null;
        public TypeObservable<int> OnNextObservable => onNextObservable;

        public void Initialize() {
            this.onNextObservable = new TypeObservable<int>();
            this.onNext.onClick.AddListener(()=>{
                this.onNextObservable.Execute(0);
            });
        }
    }
}