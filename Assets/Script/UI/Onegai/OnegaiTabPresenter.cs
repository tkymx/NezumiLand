using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace NL {
    public class OnegaiTabPresenter : DisposableMonoBehaviour {
        
        private IPlayerOnegaiRepository playerOnegaiRepository = null;

        [SerializeField]
        private OnegaiTabView onegaiTabView = null;

        [SerializeField]
        private OnegaiListPresenter onegaiListPresenter = null;

        public TypeObservable<PlayerOnegaiModel> OnCellClick { get; private set ; }

        public void Initialize (IPlayerOnegaiRepository playerOnegaiRepository) {
            this.OnCellClick = new TypeObservable<PlayerOnegaiModel>();
            this.playerOnegaiRepository = playerOnegaiRepository;

            this.onegaiTabView.Initialize();
            this.onegaiListPresenter.Initialize();

            this.disposables.Add(this.onegaiListPresenter.OnCellClick.Subscribe(playerOnegaiModel => {
                this.OnCellClick.Execute(playerOnegaiModel);
            }));

            this.disposables.Add(this.onegaiTabView.OnTapTabObservable.Subscribe(onegaiState => {
                this.SelectTab (onegaiState);
            }));
        }

        private void SelectTab (OnegaiState onegaiState) {
            var displayableOnegais = playerOnegaiRepository.GetSub(onegaiState).ToList();
            this.onegaiListPresenter.SetElement (displayableOnegais);
        }

        public void ReLoad () {
            this.SelectTab (OnegaiState.UnLock);
            this.onegaiTabView.Tap(0);
            onegaiListPresenter.ReLoad ();
        }
    }
}