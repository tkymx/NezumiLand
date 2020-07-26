using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NL
{
    public class OnegaiTabView : MonoBehaviour
    {
        [SerializeField]
        private List<Button> tabButtons = null;

        [SerializeField]
        private List<OnegaiType> displayOnegaiType = null;

        [SerializeField]
        private Button clearOnegaiButton = null;

        public TypeObservable<int> OnClearObservable { get; private set; }
        public TypeObservable<int> OnCloseObservable { get; private set; }
        public TypeObservable<OnegaiType> OnTapTabObservable { get; private set; }

        public void Initialize()
        {
            Debug.Assert(tabButtons.Count == this.displayOnegaiType.Count, "OnegaiTabの要素が異なります。");

            this.OnCloseObservable = new TypeObservable<int>();
            this.OnClearObservable = new TypeObservable<int>();
            this.OnTapTabObservable = new TypeObservable<OnegaiType>();

            for (int buttonIndex = 0; buttonIndex < this.tabButtons.Count; buttonIndex++)
            {
                var selectedButtonIndex = buttonIndex;
                this.tabButtons[selectedButtonIndex].onClick.AddListener(() => {
                    this.OnTapTabObservable.Execute(this.displayOnegaiType[selectedButtonIndex]);
                    this.Tap(selectedButtonIndex);
                });
            }

            this.clearOnegaiButton.onClick.AddListener(() => {
                this.OnClearObservable.Execute(0);
            });
        }

        // タップしたボタンは押せないようにする
        public void Tap (int tapedButtonIndex) 
        {
            for (int buttonIndex = 0; buttonIndex < this.tabButtons.Count; buttonIndex++)
            {
                this.tabButtons[buttonIndex].interactable = buttonIndex != tapedButtonIndex;
            }
        }
    }    
}

