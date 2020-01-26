using System;

namespace  NL
{
    public interface IAppearCharacterLifeDirector {
        IObservable<int> OnTouch();
        void OnInitializeView(AppearCharacterView appearCharacterView);
        void OnCreate();
        void OnRemove();        
    }
}