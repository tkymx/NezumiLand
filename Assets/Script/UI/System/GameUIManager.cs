using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public class GameUIManager : MonoBehaviour {
        [SerializeField]
        private MonoTabPresenter monoTabPresenter = null;
        public MonoTabPresenter MonoTabPresenter => monoTabPresenter;

        [SerializeField]
        private ArrangementMenuUIPresenter arrangementMenuUIPresenter = null;
        public ArrangementMenuUIPresenter ArrangementMenuUIPresenter => arrangementMenuUIPresenter;

        [SerializeField]
        private FieldActionUIPresenter fieldActionUIPresenter = null;
        public FieldActionUIPresenter FieldActionUIPresenter => fieldActionUIPresenter;

        [SerializeField]
        private SatisfactionPresenter satisfactionPresenter = null;

        [SerializeField]
        private ConversationPresenter conversationPresenter = null;
        public ConversationPresenter ConversationPresenter => conversationPresenter;

        public void Initialize (IPlayerOnegaiRepository playerOnegaiRepository) {
            this.monoTabPresenter.Initialize ();
            this.arrangementMenuUIPresenter.Initialize (playerOnegaiRepository);
            this.fieldActionUIPresenter.Initialize ();
            this.satisfactionPresenter.Initialize (playerOnegaiRepository);
            this.conversationPresenter.Initialize();
        }
    }
}