using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NL
{
    public class SelectModeUIView : MonoBehaviour
    {
        [SerializeField]
        private Button parkOpenSelectButton = null;

        public TypeObservable<int> OnClickParkOpenSelectObservable { get; private set; }

        public void Initialize() {
            this.OnClickParkOpenSelectObservable = new TypeObservable<int>();
            this.parkOpenSelectButton.onClick.AddListener(()=>{
                this.OnClickParkOpenSelectObservable.Execute(0);
            });
        }
    }   
}