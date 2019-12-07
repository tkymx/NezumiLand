using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NL
{
    public class RewardMonoView : MonoBehaviour
    {
        [SerializeField]
        Text releaseConditionText = null;

        [SerializeField]
        Button closeButton = null;

        public TypeObservable<int> OnClickCloseObservable { get; private set; }

        public void Initialize() {
            this.OnClickCloseObservable = new TypeObservable<int>();
            this.closeButton.onClick.AddListener(()=>{
                this.OnClickCloseObservable.Execute(0);
            });
        }

        public void UpdateView(string releaseConditionText) {
            this.releaseConditionText.text = releaseConditionText;
        }
    }   
}