using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  NL
{
    public class PlayingMonoState : IState
    {
        private AppearCharacterViewModel appearCharacterViewModel;

        public PlayingMonoState (AppearCharacterViewModel appearCharacterViewModel) {
            this.appearCharacterViewModel = appearCharacterViewModel;
        }

        public void onEnter () {

        }

        private bool isAlivable () {
            return !this.appearCharacterViewModel.IsPlayingFinish();
        }

        public IState onUpdate () {
            this.appearCharacterViewModel.UpdatePlaying();
            if (!isAlivable ()) {

                // お金の追加
                var earnCurrency = new Currency(10);
                GameManager.Instance.EarnCurrencyManager.CreateOrAdd(appearCharacterViewModel.PlayerAppearCharacterViewModel.PlayerArrangementTargetModel, earnCurrency);

                // パーク開放時はハートを設置
                GameManager.Instance.ParkOpenManager.AddHeart(1);
                
                return new GoAwayState(appearCharacterViewModel);
            }
            return null;
        }

        public void onExit () {

        }
    }    
}
