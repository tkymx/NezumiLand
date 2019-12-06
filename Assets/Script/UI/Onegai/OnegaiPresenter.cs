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
        private OnegaiDetailView onegaiDetailView = null;

        [SerializeField]
        private Button closeButton = null;

        private IPlayerOnegaiRepository playerOnegaiRepository;

        private List<IDisposable> disposables = null;

        public void Initialize (IPlayerOnegaiRepository playerOnegaiRepository) {

            this.playerOnegaiRepository = playerOnegaiRepository;
            this.disposables = new List<IDisposable>();

            this.onegaiListPresenter.Initialize ();
            this.onegaiListPresenter.SetElement(playerOnegaiRepository.GetAll ().ToList ());
            this.disposables.Add(this.onegaiListPresenter.OnCellClick.Subscribe(playerOnegaiModel => {
                this.ShowDetail(playerOnegaiModel);
            }));

            this.onegaiDetailView.Initialize();
            this.disposables.Add(this.onegaiDetailView.OnBack.Subscribe(_ => {
                this.CloseDetail();
            }));

            closeButton.onClick.AddListener (() => {
                this.Close ();
            });

            this.Close ();
            this.CloseDetail();
        }

        private void ShowDetail(PlayerOnegaiModel playerOnegaiModel)
        {
            this.onegaiDetailView.UpdateCell(
                playerOnegaiModel.OnegaiModel.Title,
                playerOnegaiModel.OnegaiModel.Detail,
                playerOnegaiModel.OnegaiModel.Author
            );
            this.onegaiDetailView.gameObject.SetActive(true);
        }

        private void CloseDetail()
        {
            this.onegaiDetailView.gameObject.SetActive(false);
        }

        public override void onPrepareShow () {
            base.onPrepareShow ();
            this.onegaiListPresenter.SetElement (playerOnegaiRepository.GetAll ().ToList ());
            onegaiListPresenter.ReLoad ();
        }

        private void OnDestroy() {
            if (this.disposables != null) {
                foreach (var disposable in this.disposables)
                {
                    disposable.Dispose();                    
                }
            }            
        }
    }
}