using System;

namespace  NL
{
    /// <summary>
    /// 開放機能で訪れることになったキャラクターのディレクター
    /// </summary>
    public class AppearPlayingCharacterLifeDirector : IAppearCharacterLifeDirector 
    {
        private AppearCharacterViewModel appearCharacterViewModel;
        private PlayerAppearPlayingCharacterDirectorModel playerAppearPlayingCharacterDirectorModel;

        public AppearPlayingCharacterLifeDirector(AppearCharacterViewModel appearCharacterViewModel, PlayerAppearPlayingCharacterDirectorModel playerAppearPlayingCharacterDirectorModel)
        {
            this.appearCharacterViewModel = appearCharacterViewModel;
            this.playerAppearPlayingCharacterDirectorModel = playerAppearPlayingCharacterDirectorModel;
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
                // TODO : 現状適応な遊具に歩いていっているので修正したい。
                var arrangemenTargetIndex = UnityEngine.Random.Range(0,arrangementTargetStore.Count);                        
                GameManager.Instance.AppearCharacterManager.SetTargetArrangement(
                    this.appearCharacterViewModel.PlayerAppearCharacterViewModel, 
                    arrangementTargetStore[arrangemenTargetIndex].PlayerArrangementTargetModel);
                
                this.appearCharacterViewModel.InterruptState(AppearCharacterState.GoMono);
            }
        }        
    }
}