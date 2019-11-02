using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NL
{
    public class MonoListCellView : MonoBehaviour
    {
        [SerializeField]
        private Text monoName = null;

        [SerializeField]
        private Text makingFree = null;

        [SerializeField]
        private Button cellButton = null;

        public TypeObservable<int> OnClick { get; private set; }

        public void Initialize()
        {
            OnClick = new TypeObservable<int>();
            cellButton.onClick.AddListener(() =>
            {
                OnClick.Execute(0);
            });
        }

        public void UpdateCell(string name, Currency free)
        {
            this.monoName.text = name;
            this.makingFree.text = free.Value.ToString() + "yen";
        }

        public void Enable()
        {
            this.cellButton.interactable = true;
        }

        public void DiasbleForLowFee()
        {
            this.cellButton.interactable = false;
        }
    }
}
