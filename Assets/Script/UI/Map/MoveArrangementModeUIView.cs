using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NL
{
    public class MoveArrangementModeUIView : MonoBehaviour
    {
        [SerializeField]
        private Button back = null;

        public TypeObservable<int> OnBackObservable { get; private set; }

        public void Initialize() {
            this.OnBackObservable = new TypeObservable<int>();
            this.back.onClick.AddListener(()=>{
                this.OnBackObservable.Execute(0);
            });
        }
    }   
}