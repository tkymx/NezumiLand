using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class GameUIManager : MonoBehaviour
    {
        [SerializeField]
        private MonoListPresenter monoListPresenter = null;
        public MonoListPresenter MonoListPresenter => monoListPresenter;

        [SerializeField]
        private ArrangementMenuUIPresenter arrangementMenuUIPresenter = null;
        public ArrangementMenuUIPresenter ArrangementMenuUIPresenter => arrangementMenuUIPresenter;

        [SerializeField]
        private WalletPresenter walletPresenter = null;
        public WalletPresenter WalletPresenter => walletPresenter;

        [SerializeField]
        private ArrangementModeUIPresenter arrangementModeUIPresenter = null;
        public ArrangementModeUIPresenter ArrangementModeUIPresenter => arrangementModeUIPresenter;

        public void Initialize()
        {
            this.monoListPresenter.Initialize(GameContextMap.DefaultMap.MenuSelectModeContext);
            this.arrangementMenuUIPresenter.Initialize();
            this.arrangementModeUIPresenter.Initialize();
        }
    }
}
