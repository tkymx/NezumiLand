using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NL
{
    public class RemoveArrangementModeUIView : MonoBehaviour
    {
        [SerializeField]
        private Button back = null;

        [SerializeField]
        private Button removeButton = null;

        [SerializeField]
        private Text removeCostText = null;

        [SerializeField]
        private Color defaultColor = Color.white;

        [SerializeField]
        private Color warningColor = Color.red;

        public TypeObservable<int> OnBackObservable { get; private set; }

        public TypeObservable<int> OnRemoveObservable { get; private set; }

        public void Initialize() {
            this.OnBackObservable = new TypeObservable<int>();
            this.back.onClick.AddListener(()=>{
                this.OnBackObservable.Execute(0);
            });
            this.OnRemoveObservable = new TypeObservable<int>();
            this.removeButton.onClick.AddListener(()=>{
                this.OnRemoveObservable.Execute(0);
            });
        }

        public void UpdateView(string cost, bool isCostOver) {
            this.removeCostText.text = cost;
            this.removeButton.interactable = !isCostOver;
            this.removeCostText.color = isCostOver ? warningColor : defaultColor; 
        }
    }   
}