using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace NL
{
    public class ParkOpenPromotion : Disposable
    {
        /// <summary>
        /// 看板を表示する
        /// </summary>
        /// <returns>すべてタップされたあとの見込み客数</returns>
        public IObservable<int> ShowPromotion()
        {
            // 見込み客UIの表示
            GameManager.Instance.GameUIManager.ParkOpenPromotionPresenter.Show();

            // 宣伝の作成
            var promotionCreater = GameManager.Instance.MonoManager.GenerateMonoPromotionCreater();
            promotionCreater.ShowPromotion();

            // タップした数だけ加算
            int allPromotionCount = 0;
            GameManager.Instance.GameUIManager.ParkOpenPromotionPresenter.SetAllPromotionCount(allPromotionCount);

            this.disposables.Add(promotionCreater.OnTouchPromotion.Subscribe(monoInfo => {
                allPromotionCount += monoInfo.PlayerMonoViewModel.MonoInfo.PromotionCount;
                GameManager.Instance.GameUIManager.ParkOpenPromotionPresenter.SetAllPromotionCount(allPromotionCount);
            }));

            return promotionCreater.OnAllTouchPromotion
                .Do(_ => {

                    // 見込み客UIを閉じる
                    GameManager.Instance.GameUIManager.ParkOpenPromotionPresenter.Close();

                    this.ClearDisposable();
                })
                .Select(_ => {
                    return allPromotionCount;
                });
        }
    }
}
