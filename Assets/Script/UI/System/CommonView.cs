using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NL
{
    public class CommonView : MonoBehaviour
    {
        [SerializeField]
        private Text title = null;

        [SerializeField]
        private Text contents = null;

        [SerializeField]
        private Button back = null;

        public TypeObservable<int> OnBackObservable { get; private set; }

        public void Initialize() {
            this.OnBackObservable = new TypeObservable<int>();
            this.back.onClick.AddListener(()=>{
                this.OnBackObservable.Execute(0);
            });
        }

        public void UpdateView(string title, string contents) {
            this.title.text = title;
            this.contents.text = contents;
        }
    }   
}