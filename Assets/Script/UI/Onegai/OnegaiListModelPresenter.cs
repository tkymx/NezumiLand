using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace NL {
    public class OnegaiListModelPresenter : UiWindowPresenterBase {
        
        private IPlayerOnegaiRepository playerOnegaiRepository = null;

        [SerializeField]
        private OnegaiListModelView onegaiListModelView = null;

        [SerializeField]
        private OnegaiListPresenter onegaiListPresenter = null;

        public void Initialize (IPlayerOnegaiRepository playerOnegaiRepository) {
            this.playerOnegaiRepository = playerOnegaiRepository;

            this.onegaiListModelView.Initialize();
            this.onegaiListPresenter.Initialize();

            this.disposables.Add(this.onegaiListPresenter.OnCellClick.Subscribe(playerOnegaiModel => {
                this.ShowDetail(playerOnegaiModel);
            }));

            this.disposables.Add(this.onegaiListModelView.OnBackObservable.Subscribe(_ => {
                this.Close();
                this.CloseDetail();
            }));

            this.Close();
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

        public void ReLoad () {
            this.onegaiListPresenter.ReLoad ();
        }

        public void SetContentAll(OnegaiState onegaiState, string title) {
            var displayableOnegais = playerOnegaiRepository.GetAll(onegaiState).ToList();
            this.onegaiListPresenter.SetElement (displayableOnegais);
            this.onegaiListModelView.SetTitle(title);
        }

        public void SetContent(OnegaiType onegaiType, OnegaiState onegaiState, string title) {
            var displayableOnegais = playerOnegaiRepository.GetFromType(onegaiType, onegaiState).ToList();
            this.onegaiListPresenter.SetElement (displayableOnegais);
            this.onegaiListModelView.SetTitle(title);
        }

        public override void onPrepareShow () {
            base.onPrepareShow ();
            this.ReLoad ();
        }

        public override void onPrepareClose() {
            base.onPrepareClose();
            this.CloseDetail();
        }        
    }
}