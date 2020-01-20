using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace NL {

    /// <summary>
    /// インスタンス化された稼ぎコインの行動を管理
    /// </summary>
    public class EarnCurrencyViewModel
    {
        private EarnCurrencyView earnCurrencyView;

        public PlayerEarnCurrencyModel playerEarnCurrencyModel { get; private set; }

        private List<IDisposable> disposables = new List<IDisposable>();

        public EarnCurrencyViewModel(EarnCurrencyView earnCurrencyView, PlayerEarnCurrencyModel playerEarnCurrencyModel)
        {
            this.earnCurrencyView = earnCurrencyView;            
            this.playerEarnCurrencyModel = playerEarnCurrencyModel;

            // キャラクタがタップされた時
            disposables.Add(earnCurrencyView.OnSelectObservable
                .Subscribe(_ => {

                    // 稼いだお金を財布に格納する
                    GameManager.Instance.Wallet.Push(this.playerEarnCurrencyModel.EarnCurrency);
                    GameManager.Instance.EffectManager.PlayEarnEffect(this.playerEarnCurrencyModel.EarnCurrency, earnCurrencyView.transform.position);
                    GameManager.Instance.EarnCurrencyManager.EnqueueRemove(this);
                }));
        }

        public void UpdateByFrame()
        {
        }

        public void Dispose () 
        {
            Object.DisAppear(earnCurrencyView.gameObject);

            // dispose を駆逐
            foreach (var disposable in disposables)
            {
                disposable.Dispose();
            }
            disposables.Clear();
        }
    }
}