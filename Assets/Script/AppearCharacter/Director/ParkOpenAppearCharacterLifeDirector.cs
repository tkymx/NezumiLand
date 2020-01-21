using System;

namespace  NL
{
    /// <summary>
    /// 開放機能で訪れることになったキャラクターのディレクター
    /// </summary>
    public class ParkOpenAppearCharacterLifeDirector : IAppearCharacterLifeDirector 
    {
        public IObservable<int> OnTouch()
        {
            return new ImmediatelyObservable<int>(0);
        }
        public void OnCreate()
        {

        }
        public void OnRemove()
        {
            
        }
    }
}