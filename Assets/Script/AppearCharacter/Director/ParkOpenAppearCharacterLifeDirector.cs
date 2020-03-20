using System;

namespace  NL
{
    /// <summary>
    /// 開放機能で訪れることになったキャラクターのディレクター
    /// </summary>
    public class ParkOpenAppearCharacterLifeDirector : IAppearCharacterLifeDirector 
    {
        private AppearCharacterViewModel appearCharacterViewModel;
        private PlayerAppearParkOpenCharacterDirectorModel playerAppearParkOpenCharacterDirectorModel;

        public ParkOpenAppearCharacterLifeDirector(AppearCharacterViewModel appearCharacterViewModel, PlayerAppearParkOpenCharacterDirectorModel playerAppearParkOpenCharacterDirectorModel)
        {
            this.appearCharacterViewModel = appearCharacterViewModel;
            this.playerAppearParkOpenCharacterDirectorModel = playerAppearParkOpenCharacterDirectorModel;
        }

        public IObservable<int> OnTouch()
        {
            return new ImmediatelyObservable<int>(0);
        }
        public void OnInitializeView(AppearCharacterView appearCharacterView)
        {
            appearCharacterView.SetConversationNotifierEnabled(false);
        }
        public void OnCreate()
        {
            this.SetInitialState();
        }
        public void OnUpdateByFrame()
        {
        }

        public void OnRemove()
        {
            
        }

        public void SetInitialState () {
            var arrangementTargetStore = GameManager.Instance.ArrangementManager.ArrangementTargetStore;
            if (arrangementTargetStore.Count > 0) {  
                // ターゲットの設定
                var arrangemenTargetIndex = UnityEngine.Random.Range(0,arrangementTargetStore.Count);                        
                GameManager.Instance.AppearCharacterManager.SetTargetArrangement(
                    this.appearCharacterViewModel.PlayerAppearCharacterViewModel, 
                    arrangementTargetStore[arrangemenTargetIndex].PlayerArrangementTargetModel);
                
                this.appearCharacterViewModel.InterruptState(AppearCharacterState.GoMono);
            }
        }        
    }
}