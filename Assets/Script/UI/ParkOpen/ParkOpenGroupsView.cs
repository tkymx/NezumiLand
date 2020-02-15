using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NL
{
    public class ParkOpenGroupsView : MonoBehaviour
    {
        [SerializeField]
        private Image background = null;

        [SerializeField]
        private Button back = null;

        public TypeObservable<int> OnBackObservable { get; private set; }

        public void Initialize() {
            this.OnBackObservable = new TypeObservable<int>();
            this.back.onClick.AddListener(()=>{
                this.OnBackObservable.Execute(0);
            });
        }

        public void UpdateView(Sprite background) {
            this.background.sprite = background;
        }
    }   
}