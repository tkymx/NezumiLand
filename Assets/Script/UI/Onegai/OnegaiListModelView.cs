using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NL
{
    public class OnegaiListModelView : MonoBehaviour
    {
        [SerializeField]
        private Text title = null;

        [SerializeField]
        private Button back = null;

        public TypeObservable<int> OnBackObservable { get; private set; }

        public void Initialize() {
            this.OnBackObservable = new TypeObservable<int>();
            this.back.onClick.AddListener(()=>{
                this.OnBackObservable.Execute(0);
            });
        }

        public void SetTitle(string title)
        {
            this.title.text = title;
        }
    }   
}