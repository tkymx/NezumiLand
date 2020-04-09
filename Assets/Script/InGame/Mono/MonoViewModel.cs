using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace NL {
    public class MonoViewModel : IDisposable{
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

        public void SetPosition (Vector3 position)
        {
            this.monoView.SetPosition(position);
        }

        // プロモーションのインスタンス
        private GameObject promotionInstance;
        public bool HasPromotion => promotionInstance != null;

        /// <summary>
        /// 宣伝用の看板を作成
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public IObservable<int> ShowPromotion(Camera camera, GameObject root)
        {
            // あれば消す
            if (this.HasPromotion) {
                RemovePromotion();
            }

            // インスタンスを作成
            var promotionPrefab = ResourceLoader.LoadModel("mono_promotion");
            promotionInstance = Object.AppearToFloor(promotionPrefab, root, this.monoView.transform.localPosition);
            var monoPromotionView = promotionInstance.GetComponent<MonoInfoPromotionView>();
            monoPromotionView.Initialize(camera);
            monoPromotionView.UpdateView(this.playerMonoViewModel.MonoInfo.PromotionCount.ToString());
            Debug.Assert(monoPromotionView != null, "mono info promotion view を設定します。");

            return monoPromotionView.OnTouchObservable
                .Do(_ => {
                    RemovePromotion();
                });
        }

        private void RemovePromotion()
        {
            Object.DisAppear(this.promotionInstance);
            this.promotionInstance = null;
        }

        public void Dispose()
        {
            if (this.monoView) {
                Object.DisAppear(this.monoView.gameObject);
                this.monoView = null;
            }

            if (this.HasPromotion) {
                RemovePromotion();
            }

        }
    }
}