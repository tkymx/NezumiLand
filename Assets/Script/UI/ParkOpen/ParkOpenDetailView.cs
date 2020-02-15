using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NL
{
    public class ParkOpenDetailView : MonoBehaviour
    {
        [SerializeField]
        private Text title = null;

        [SerializeField]
        private Text coinCount = null;

        [SerializeField]
        private Text itemCount = null;

        [SerializeField]
        private Button startButton = null;

        public TypeObservable<int> OnStartObservable { get; private set; }

        public void Initialize() {
            this.OnStartObservable = new TypeObservable<int>();
            this.startButton.onClick.AddListener(()=>{
                this.OnStartObservable.Execute(0);
            });
        }

        public void UpdateView(string title, string coinCount, string itemCount) {
            this.title.text = title;
            this.coinCount.text = coinCount;
            this.itemCount.text = itemCount;
        }
    }   
}