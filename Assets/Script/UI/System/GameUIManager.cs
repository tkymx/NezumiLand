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

        [SerializeField]
        private RewardPresenter rewardPresenter = null;
        public RewardPresenter RewardPresenter => rewardPresenter;

        [SerializeField]
        private OnegaiPresenter onegaiPresenter = null;

        public OnegaiPresenter OnegaiPresenter => onegaiPresenter;

        [SerializeField]
        private OnegaiDetailPresenter onegaiDetailPresenter = null;

        public OnegaiDetailPresenter OnegaiDetailPresenter => onegaiDetailPresenter;

        [SerializeField]
        private RewardOnegaiPresenter rewardOnegaiPresenter = null;
        public RewardOnegaiPresenter RewardOnegaiPresenter => rewardOnegaiPresenter;


        public void Initialize (OnegaiRepository onegaiRepository, IPlayerOnegaiRepository playerOnegaiRepository) {
            this.monoTabPresenter.Initialize ();
            this.arrangementMenuUIPresenter.Initialize (playerOnegaiRepository);
            this.fieldActionUIPresenter.Initialize ();
            this.satisfactionPresenter.Initialize (playerOnegaiRepository);
            this.conversationPresenter.Initialize();
            this.rewardPresenter.Initialize();
            this.onegaiPresenter.Initialize(playerOnegaiRepository);
            this.onegaiDetailPresenter.Initialize();
            this.rewardOnegaiPresenter.Initialize(onegaiRepository);
        }
    }
}