using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace NL {
    public class OnegaiPresenter : UiWindowPresenterBase {

        [SerializeField]
        private OnegaiListPresenter onegaiListPresenter = null;

        [SerializeField]
        private Button closeButton = null;

        private IPlayerOnegaiRepository playerOnegaiRepository;

        public void Initialize (IPlayerOnegaiRepository playerOnegaiRepository) {

            this.playerOnegaiRepository = playerOnegaiRepository;
            this.disposables = new List<IDisposable>();

            this.onegaiListPresenter.Initialize ();
            this.disposables.Add(this.onegaiListPresenter.OnCellClick.Subscribe(playerOnegaiModel => {
                this.ShowDetail(playerOnegaiModel);
            }));

            this.closeButton.onClick.AddListener (() => {
                this.Close ();
                this.CloseDetail();
            });

            this.Close ();
            this.CloseDetail();
        }

        private void ShowDetail(PlayerOnegaiModel playerOnegaiModel)
        {
            GameManager.Instance.GameUIManager.OnegaiDetailPresenter.SetOnegaiDetail(playerOnegaiModel);
            GameManager.Instance.GameUIManager.OnegaiDetailPresenter.Show();
        }

        private void CloseDetail()
        {
            GameManager.Instance.GameUIManager.OnegaiDetailPresenter.Close();
        }

        public override void onPrepareShow () {
            base.onPrepareShow ();
            this.onegaiListPresenter.SetElement (playerOnegaiRepository.GetDisplayable ().ToList ());
            onegaiListPresenter.ReLoad ();
        }

        public override void onPrepareClose() {
            base.onPrepareClose();
            this.CloseDetail();
        }
    }
}