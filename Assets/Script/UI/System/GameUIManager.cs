using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class GameUIManager : MonoBehaviour
    {
        [SerializeField]
        private MonoTabPresenter monoTabPresenter = null;
        public MonoTabPresenter MonoTabPresenter => monoTabPresenter;

        [SerializeField]
        private ArrangementMenuUIPresenter arrangementMenuUIPresenter = null;
        public ArrangementMenuUIPresenter ArrangementMenuUIPresenter => arrangementMenuUIPresenter;

        [SerializeField]
        private FieldActionUIPresenter fieldActionUIPresenter = null;
        public FieldActionUIPresenter FieldActionUIPresenter => fieldActionUIPresenter;

        public void Initialize()
        {
            this.monoTabPresenter.Initialize();
            this.arrangementMenuUIPresenter.Initialize();
            this.fieldActionUIPresenter.Initialize();
        }
    }
}
