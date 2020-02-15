using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NL
{
    public class ParkOpenStartView : MonoBehaviour
    {
        [SerializeField]
        private Text title = null;

        [SerializeField]
        private Button startButton = null;

        [SerializeField]
        private Button backButton = null;

        public TypeObservable<int> OnStartObservable { get; private set; }
        public TypeObservable<int> OnBackObservable { get; private set; }

        public void Initialize() {
            this.OnStartObservable = new TypeObservable<int>();
            this.OnBackObservable = new TypeObservable<int>();

            this.startButton.onClick.AddListener(()=>{
                this.OnStartObservable.Execute(0);
            });
            this.backButton.onClick.AddListener(()=>{
                this.OnBackObservable.Execute(0);
            });
        }

        public void UpdateView(string title) {
            this.title.text = title;
        }
    }   
}