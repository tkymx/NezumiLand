using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace  NL
{
    public class MousePurchaseView : MonoBehaviour
    {
        [SerializeField]
        Text currentMouseStock = null;

        [SerializeField]
        Text nextMouseStock = null;

        [SerializeField]
        Text currencyCost = null;

        [SerializeField]
        Text itemCost = null;

        [SerializeField]
        Button purchaceButton = null;

        [SerializeField]
        Button closeButton = null;

        [SerializeField]
        GameObject noConsumeWarning = null;

        private TypeObservable<int> onPurchace;
        public TypeObservable<int> OnPurchace => onPurchace;

        private TypeObservable<int> onClose;
        public TypeObservable<int> OnClose => onClose;

        public void Initialize() {
            this.onClose = new TypeObservable<int>();
            this.onPurchace = new TypeObservable<int>();

            this.purchaceButton.onClick.AddListener(()=>{
                this.onPurchace.Execute(0);
            });
            this.closeButton.onClick.AddListener(()=>{
                this.onClose.Execute(0);
            });
        }

        public void UpdateView(string currentMouseMaStock, string nextMouseMaStock, string currencyCost, string itemCost) {
            this.currentMouseStock.text = currentMouseMaStock;
            this.nextMouseStock.text = nextMouseMaStock;
            this.currencyCost.text = currencyCost;
            this.itemCost.text = itemCost;
            this.noConsumeWarning.gameObject.SetActive(false);
            this.purchaceButton.interactable = true;
        }

        public void SetNoPuchaseByNoCost() {
            this.noConsumeWarning.gameObject.SetActive(true);
            this.purchaceButton.interactable = false;
        }
        public void SetNoPuchaseByMaxCount() {
            this.noConsumeWarning.gameObject.SetActive(false);
            this.purchaceButton.interactable = false;
        }
    }   
}