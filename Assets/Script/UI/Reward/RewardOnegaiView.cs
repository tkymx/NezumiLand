using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NL
{
    public class RewardOnegaiView : MonoBehaviour
    {
        [SerializeField]
        Text title = null;

        [SerializeField]
        Button detailButton = null;

        [SerializeField]
        Button closeButton = null;

        public TypeObservable<int> OnClickDetailObservable { get; private set; }
        public TypeObservable<int> OnClickCloseObservable { get; private set; }

        public void Initialize() {
            this.OnClickDetailObservable = new TypeObservable<int>();
            this.OnClickCloseObservable = new TypeObservable<int>();
            this.detailButton.onClick.AddListener(()=>{
                this.OnClickDetailObservable.Execute(0);
            });
            this.closeButton.onClick.AddListener(()=>{
                this.OnClickCloseObservable.Execute(0);
            });
        }

        public void UpdateView(string title) {
            this.title.text = title;
        }
    }   
}