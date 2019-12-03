using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace NL {
    public class OnegaiPresenter : UiWindowPresenterBase {
        [SerializeField]
        private OnegaiListPresenter onegaiListPresenter = null;

        [SerializeField]
        private Button closeButton = null;
        private IPlayerOnegaiRepository playerOnegaiRepository;

        public void Initialize (IPlayerOnegaiRepository playerOnegaiRepository) {
            this.playerOnegaiRepository = playerOnegaiRepository;

            this.onegaiListPresenter.Initialize (playerOnegaiRepository.GetAll ().ToList ());
            this.Close ();

            closeButton.onClick.AddListener (() => {
                this.Close ();
            });
        }

        public override void onPrepareShow () {
            base.onPrepareShow ();
            this.onegaiListPresenter.Initialize (playerOnegaiRepository.GetAll ().ToList ());
            onegaiListPresenter.ReLoad ();
        }
    }
}