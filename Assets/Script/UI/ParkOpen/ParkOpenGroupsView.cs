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
        private Button selectCancelButton = null;

        public TypeObservable<int> OnSelectCancelObservable { get; private set; }

        public void Initialize() {
            this.OnSelectCancelObservable = new TypeObservable<int>();
            this.selectCancelButton.onClick.AddListener(()=>{
                this.OnSelectCancelObservable.Execute(0);
            });
        }

        public void UpdateView(Sprite background) {
            this.background.sprite = background;
        }
    }   
}