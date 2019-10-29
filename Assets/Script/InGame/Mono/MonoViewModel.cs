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

        // 消去に必要な通貨
        public Currency RemoveCurrency => new Currency(100);

        public MonoViewModel(MonoView monoView)
        {
            this.monoView = monoView;
        }

        private float elapsedTime = 0;
        private float earnTime = 1;

        public void UpdateByFrame()
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > earnTime)
            {
                // ここはMonoInfo から取りたい
                var currency = new Currency(10);
                GameManager.Instance.Wallet.Push(currency);
                GameManager.Instance.EffectManager.PlayEarnEffect(currency, monoView.transform.position);
                elapsedTime = 0;
            }
        }
    }
}
