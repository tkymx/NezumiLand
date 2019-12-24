using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public class MonoViewModel {
        // monoView
        private MonoView monoView = null;
        public MonoView MonoView =>this.monoView;

        // private
        private PlayerMonoViewModel playerMonoViewModel;
        public PlayerMonoViewModel PlayerMonoViewModel => this.playerMonoViewModel;

        public int CurrentLevel => this.playerMonoViewModel.Level;

        public Currency RemoveFee => this.playerMonoViewModel.MonoInfo.RemoveFee;

        public MonoViewModel (MonoView monoView, PlayerMonoViewModel playerMonoViewModel) {
            this.monoView = monoView;
            this.playerMonoViewModel = playerMonoViewModel;
        }

        public bool ExistNextLevelUp () {
            return this.playerMonoViewModel.MonoInfo.LevelUpFee.Length > this.playerMonoViewModel.Level; // ５レベルまでなら、配列数は6
        }

        public Currency GetCurrentLevelUpFee () {
            Debug.Assert (ExistNextLevelUp (), "次のレベルがありません");
            return this.playerMonoViewModel.MonoInfo.LevelUpFee[this.playerMonoViewModel.Level];
        }

        public void LevelUp () {
            if (this.ExistNextLevelUp ()) {
                GameManager.Instance.MonoManager.LevelUp(this.playerMonoViewModel);
            }
        }

        public Satisfaction GetCurrentSatisfaction () {
            return this.playerMonoViewModel.MonoInfo.BaseSatisfaction + this.playerMonoViewModel.MonoInfo.LevelUpSatisfaction[this.playerMonoViewModel.Level - 1];
        }

        public void UpdateByFrame () { }
    }
}