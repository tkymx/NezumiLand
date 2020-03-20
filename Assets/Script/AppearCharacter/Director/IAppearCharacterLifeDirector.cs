using System;

namespace  NL
{
    public interface IAppearCharacterLifeDirector {
        IObservable<int> OnTouch();
        void OnInitializeView(AppearCharacterView appearCharacterView);
        void OnUpdateByFrame();
        void OnCreate();
        void OnRemove();        
    }
}