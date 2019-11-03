using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class MonoViewModel
    {
        // monoView
        private MonoView monoView = null;
        public MonoView MonoView => monoView;

        // private
        private MonoInfo monoInfo;

        private uint currentLevel;
        public uint CurrentLevel => currentLevel;

        public Currency RemoveFee => monoInfo.RemoveFee;

        public MonoViewModel(MonoView monoView, MonoInfo monoInfo)
        {
            this.currentLevel = 1;
            this.monoView = monoView;
            this.monoInfo = monoInfo;
        }

        public bool ExistNextLevelUp()
        {
            return monoInfo.LevelUpFee.Length > this.currentLevel;     // ５レベルまでなら、配列数は6
        }

        public Currency GetCurrentLevelUpFee()
        {
            Debug.Assert(ExistNextLevelUp(),"次のレベルがありません");
            return monoInfo.LevelUpEarn[this.currentLevel];
        }

        public Currency GetCurrentEarn()
        {
            return monoInfo.LevelUpEarn[this.currentLevel-1];
        }

        public void LevelUp()
        {
            this.currentLevel++;
            if (!this.ExistNextLevelUp())
            {
                this.currentLevel = (uint)monoInfo.LevelUpFee.Length;
            }
        }

        private float elapsedTime = 0;
        private float earnTime = 1;

        public void UpdateByFrame()
        {
            // 別クラスにするでも良い
            elapsedTime += Time.deltaTime;
            if (elapsedTime > earnTime)
            {
                var currency = this.GetCurrentEarn();
                GameManager.Instance.Wallet.Push(currency);
                GameManager.Instance.EffectManager.PlayEarnEffect(currency, monoView.transform.position);
                elapsedTime = 0;
            }
        }
    }
}
