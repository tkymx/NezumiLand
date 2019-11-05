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
        private FieldActionUIPresenter fieldActionUIPresenter = null;
        public FieldActionUIPresenter FieldActionUIPresenter => fieldActionUIPresenter;

        public void Initialize()
        {
            this.monoListPresenter.Initialize();
            this.arrangementMenuUIPresenter.Initialize();
            this.fieldActionUIPresenter.Initialize();
        }
    }
}
