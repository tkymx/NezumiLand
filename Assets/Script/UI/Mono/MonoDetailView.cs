using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NL
{
    public class MonoDetailView : MonoBehaviour
    {
        [SerializeField]
        private Text title = null;

        [SerializeField]
        private Text arragnementCurrencyCost = null;

        [SerializeField]
        private Text arragnementItemCost = null;

        [SerializeField]
        private Text arragnementCount = null;

        [SerializeField]
        private Text arragnementMaxCount = null;

        [SerializeField]
        private Text satisfaction = null;

        [SerializeField]
        private Text size = null;
        
        [SerializeField]
        private Button close = null;

        private TypeObservable<int> onClose = null;
        public TypeObservable<int> OnClose => onClose;

        public void Initialize() {
            this.onClose = new TypeObservable<int>();
            this.close.onClick.AddListener(()=>{
                this.onClose.Execute(0);
            });
        }

        public void UpdateView(string title, string arragnementCurrencyCost, string arragnementItemCost, string arragnementCount, string arragnementMaxCount, string satisfaction, string size) {
            this.title.text = title;
            this.arragnementCurrencyCost.text = arragnementCurrencyCost;
            this.arragnementItemCost.text = arragnementItemCost;
            this.arragnementCount.text = arragnementCount;
            this.arragnementMaxCount.text = arragnementMaxCount;
            this.satisfaction.text = satisfaction;
            this.size.text = size;
        }
    }    
}

