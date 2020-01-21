using System;

namespace  NL
{
    public interface IAppearCharacterLifeDirector {
        IObservable<int> OnTouch();
        void OnCreate();
        void OnRemove();        
    }
}