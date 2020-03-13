using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace NL {
    public class OnegaiPresenter : UiWindowPresenterBase {

        [SerializeField]
        private OnegaiListCellView mainCellView = null;

        [SerializeField]
        private GameObject mainAllClearIndicator = null;

        [SerializeField]
        private OnegaiTabPresenter onegaiTabPresenter = null;

        [SerializeField]
        private Button closeButton = null;

        private IPlayerOnegaiRepository playerOnegaiRepository;
        private PlayerOnegaiModel mainPlayerOnegaiModel;

        public void Initialize (IPlayerOnegaiRepository playerOnegaiRepository) {

            this.playerOnegaiRepository = playerOnegaiRepository;
            this.disposables = new List<IDisposable>();

            this.onegaiTabPresenter.Initialize (playerOnegaiRepository);
            this.mainCellView.Initialize();

            this.disposables.Add(this.onegaiTabPresenter.OnCellClick.Subscribe(playerOnegaiModel => {
                this.ShowDetail(playerOnegaiModel);
            }));

            this.closeButton.onClick.AddListener (() => {
                this.Close ();
                this.CloseDetail();
            });

            this.disposables.Add(this.mainCellView.OnClick.Subscribe(_ => {
                this.ShowDetail(this.mainPlayerOnegaiModel);
            }));

            this.Close ();
            this.CloseDetail();
        }

        private void ShowDetail(PlayerOnegaiModel playerOnegaiModel)
        {
            if (playerOnegaiModel == null) {
                return;
            }
            
            GameManager.Instance.GameUIManager.OnegaiDetailPresenter.SetOnegaiDetail(playerOnegaiModel);
            GameManager.Instance.GameUIManager.OnegaiDetailPresenter.Show();
        }

        private void CloseDetail()
        {
            GameManager.Instance.GameUIManager.OnegaiDetailPresenter.Close();
        }

        private void ReLoadCurrentMain()
        {
            this.mainPlayerOnegaiModel = this.playerOnegaiRepository.GetCurrentMain();

            if (this.mainPlayerOnegaiModel == null) {

                // すべてクリアの表記をする
                this.mainAllClearIndicator.SetActive(true);
                return;
            }

            this.mainAllClearIndicator.SetActive(false);
            mainCellView.UpdateCell(
                this.mainPlayerOnegaiModel.OnegaiModel.Title,
                this.mainPlayerOnegaiModel.OnegaiState == OnegaiState.Clear,
                this.mainPlayerOnegaiModel.HasSchedule (),
                this.mainPlayerOnegaiModel.CloseTime (),
                this.mainPlayerOnegaiModel.OnegaiModel.Satisfaction.ToString());
        }

        public override void onPrepareShow () {
            base.onPrepareShow ();
            this.ReLoadCurrentMain();
            onegaiTabPresenter.ReLoad ();
        }

        public override void onPrepareClose() {
            base.onPrepareClose();
            this.CloseDetail();
        }
    }
}