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
        private List<OnegaiState> displayOnegaiState = null;

        public TypeObservable<int> OnCloseObservable { get; private set; }
        public TypeObservable<OnegaiState> OnTapTabObservable { get; private set; }

        public void Initialize()
        {
            Debug.Assert(tabButtons.Count == this.displayOnegaiState.Count, "OnegaiTabの要素が異なります。");

            this.OnCloseObservable = new TypeObservable<int>();
            this.OnTapTabObservable = new TypeObservable<OnegaiState>();

            for (int buttonIndex = 0; buttonIndex < this.tabButtons.Count; buttonIndex++)
            {
                var selectedButtonIndex = buttonIndex;
                this.tabButtons[selectedButtonIndex].onClick.AddListener(() => {
                    this.OnTapTabObservable.Execute(this.displayOnegaiState[selectedButtonIndex]);
                    this.Tap(selectedButtonIndex);
                });
            }
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

