using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NL
{
    public class ParkOpenInitialCommentView : DisposableMonoBehaviour
    {
        [SerializeField]
        private AnimationView openAnimation;

        [SerializeField]
        private AnimationView closeAnimation;

        public TypeObservable<int> OnEndOpenAnimation { get; private set; }
        public TypeObservable<int> OnEndCloseAnimation { get; private set; }

        public void Initialize() {
            this.OnEndOpenAnimation = new TypeObservable<int>();            
            this.OnEndCloseAnimation = new TypeObservable<int>();

            this.openAnimation.Initialize();
            this.closeAnimation.Initialize();

            this.disposables.Add(this.openAnimation.OnComplated.Subscribe(_ => {
                this.OnEndOpenAnimation.Execute(_);
            }));
 
            this.disposables.Add(this.closeAnimation.OnComplated.Subscribe(_ => {
                this.OnEndCloseAnimation.Execute(_);
            }));
        }

        public void UpdateByFrame()
        {
            this.openAnimation.UpdateByFrame();
            this.closeAnimation.UpdateByFrame();
        }


        public void StartOpen()
        {
            this.openAnimation.StartAnimation();
        }

        public void StartClose()
        {
            this.closeAnimation.StartAnimation();
        }
    }   
}