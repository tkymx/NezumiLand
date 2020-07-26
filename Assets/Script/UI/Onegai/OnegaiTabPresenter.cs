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
        public TypeObservable<int> OnClearButtonClick { get; private set; }

        public void Initialize (IPlayerOnegaiRepository playerOnegaiRepository) {
            this.OnCellClick = new TypeObservable<PlayerOnegaiModel>();
            this.OnClearButtonClick = new TypeObservable<int>();

            this.playerOnegaiRepository = playerOnegaiRepository;

            this.onegaiTabView.Initialize();
            this.onegaiListPresenter.Initialize();

            this.disposables.Add(this.onegaiListPresenter.OnCellClick.Subscribe(playerOnegaiModel => {
                this.OnCellClick.Execute(playerOnegaiModel);
            }));

            this.disposables.Add(this.onegaiTabView.OnTapTabObservable.Subscribe(onegaiType => {
                this.SelectTab (onegaiType);
            }));

            this.disposables.Add(this.onegaiTabView.OnClearObservable.Subscribe(_ => {
                this.OnClearButtonClick.Execute(0);
            }));
        }

        private void SelectTab (OnegaiType onegaiType) {
            var displayableOnegais = playerOnegaiRepository.GetFromType(onegaiType, OnegaiState.UnLock).ToList();
            this.onegaiListPresenter.SetElement (displayableOnegais);
        }

        public void ReLoad () {
            this.SelectTab (OnegaiType.Sub);
            this.onegaiTabView.Tap(0);
            onegaiListPresenter.ReLoad ();
        }
    }
}