using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class MousePurchasePresenter : UiWindowPresenterBase
    {
        private IMousePurchaceTableRepository mousePurchaceTableRepository = null;
        private IPlayerMouseStockRepository playerMouseStockRepository = null;

        [SerializeField]
        private MousePurchaseView mousePurchaseView = null;

        public void Initialize(IMousePurchaceTableRepository mousePurchaceTableRepository, IPlayerMouseStockRepository playerMouseStockRepository) {
            this.mousePurchaceTableRepository = mousePurchaceTableRepository;
            this.playerMouseStockRepository = playerMouseStockRepository;

            this.mousePurchaseView.Initialize();
            this.disposables.Add (this.mousePurchaseView.OnClose.Subscribe(_ => {
                this.Close();
            }));
            this.disposables.Add (this.mousePurchaseView.OnPurchace.Subscribe(_ => {
                var mousePurchaceTableModel = this.mousePurchaceTableRepository.Get((uint)(FetchCurrentCount().Value+1));
                var amount = mousePurchaceTableModel.GetMousePurchaseResourceAmount();
                if(MousePurchaseResourceHelper.IsConsume(amount)) {

                    // 素材を消費
                    MousePurchaseResourceHelper.Consume(amount);

                    // ネズミの数を増やす
                    this.IncrementMouseStockCount();

                    // 見た目を変更する
                    this.UpdateView();
                }
            }));
            this.Close();
        }

        private void IncrementMouseStockCount() {
            // プレイヤーデータを更新
            var playerMouseStockModel = this.playerMouseStockRepository.GetOwn();
            playerMouseStockModel.Increment();
            this.playerMouseStockRepository.Store(playerMouseStockModel);
            // 表示を更新
            GameManager.Instance.MouseStockManager.FetchMouseStockCount();
        }

        public override void onPrepareShow() {
            this.UpdateView();
        }

        private MouseOrderAmount FetchCurrentCount() {
            var currentMouseStockCount = this.playerMouseStockRepository.GetOwn().MouseStockCount;
            return currentMouseStockCount;
        }

        private bool IsMaxCount() {
            var currentMouseStockCount = FetchCurrentCount();
            return this.mousePurchaceTableRepository.MaxPurchaseCount() <= currentMouseStockCount;
        }

        private void UpdateView () {
            var currentMouseStockCount = FetchCurrentCount();
            var nextMouseStockCount = currentMouseStockCount + 1;

            if (IsMaxCount()) {
                this.mousePurchaseView.UpdateView(
                    currentMouseStockCount.ToString(),
                    "-",
                    "-",
                    "-"
                );
                this.mousePurchaseView.SetNoPuchaseByMaxCount();
            }
            else {
                var mousePurchaceTableModel = this.mousePurchaceTableRepository.Get((uint)nextMouseStockCount.Value);
                this.mousePurchaseView.UpdateView(
                    currentMouseStockCount.ToString(),
                    nextMouseStockCount.ToString(),
                    mousePurchaceTableModel.CurrencyCost.ToString(),
                    mousePurchaceTableModel.ItemConst.ToString()
                );

                var canPurchase = MousePurchaseResourceHelper.IsConsume(mousePurchaceTableModel.GetMousePurchaseResourceAmount());
                if (!canPurchase) {
                    this.mousePurchaseView.SetNoPuchaseByNoCost();
                }
            }
        }
    }   
}