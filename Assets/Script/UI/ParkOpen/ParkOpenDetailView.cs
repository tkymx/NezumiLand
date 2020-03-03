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
        private Text description = null;

        [SerializeField]
        private Button startButton = null;

        [SerializeField]
        private GameObject specialIndicator = null;

        [SerializeField]
        private Text goalHeart = null;

        public TypeObservable<int> OnStartObservable { get; private set; }

        public void Initialize() {
            this.OnStartObservable = new TypeObservable<int>();
            this.startButton.onClick.AddListener(()=>{
                this.OnStartObservable.Execute(0);
            });
        }

        public void UpdateView(string title, string description, string goalHeart, bool isSpecial) {
            this.title.text = title;
            this.description.text = description;
            this.specialIndicator.SetActive(isSpecial);
            this.goalHeart.text = "目標ハート " + goalHeart;
        }
    }   
}