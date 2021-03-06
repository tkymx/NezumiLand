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

        [SerializeField]
        private RewardMonoPresenter rewardMonoInfoPresenter = null;
        public RewardMonoPresenter RewardMonoInfoPresenter => rewardMonoInfoPresenter;

        [SerializeField]
        private CommonPresenter commonPresenter = null;
        public CommonPresenter CommonPresenter => commonPresenter;

        [SerializeField]
        private MonoDetailPresenter monoDetailPresenter = null;
        public MonoDetailPresenter MonoDetailPresenter => monoDetailPresenter;

        [SerializeField]
        private MousePurchasePresenter mousePurchasePresenter = null;
        public MousePurchasePresenter MousePurchasePresenter => mousePurchasePresenter;

        [SerializeField]
        private ToolBarManager toolBarmanager = null;
        public ToolBarManager ToolBarManager => toolBarmanager;

        [SerializeField]
        private OnegaiConditionNotificationPresenter onegaiConditionNotificationPresenter = null;
        public OnegaiConditionNotificationPresenter OnegaiConditionNotificationPresenter => onegaiConditionNotificationPresenter;

        [SerializeField]
        private ImagePresenter imagePresenter = null;
        public ImagePresenter ImagePresenter => imagePresenter;

        [SerializeField]
        private OnegaiListModelPresenter onegaiListModelPresenter = null;
        public OnegaiListModelPresenter OnegaiListModelPresenter => onegaiListModelPresenter;

        [SerializeField]
        private ArrangementModeUIPresenter arrangementModeUIPresenter = null;
        public ArrangementModeUIPresenter ArrangementModeUIPresenter => arrangementModeUIPresenter;

        [SerializeField]
        private SelectModeUIPresenter selectModeUIPresenter = null;
        public SelectModeUIPresenter SelectModeUIPresenter => selectModeUIPresenter;

        [SerializeField]
        private RemoveArrangementModeUIPresenter removeArrangementModeUIPresenter = null;
        public RemoveArrangementModeUIPresenter RemoveArrangementModeUIPresenter => this.removeArrangementModeUIPresenter;

        [SerializeField]
        private MoveArrangementModeUIPresenter moveArrangementModeUIPresenter = null;
        public MoveArrangementModeUIPresenter MoveArrangementModeUIPresenter => this.moveArrangementModeUIPresenter;

        public void Initialize (
            OnegaiRepository onegaiRepository, 
            IPlayerOnegaiRepository playerOnegaiRepository, 
            IMonoInfoRepository monoInfoRepository, 
            IPlayerMonoInfoRepository playerMonoInfoRepository, 
            IMousePurchaceTableRepository mousePurchaceTableRepository,
            IPlayerMouseStockRepository playerMouseStockRepository
        ) {
            this.monoTabPresenter.Initialize (playerMonoInfoRepository);
            this.arrangementMenuUIPresenter.Initialize (playerOnegaiRepository);
            this.fieldActionUIPresenter.Initialize ();
            this.satisfactionPresenter.Initialize (playerOnegaiRepository);
            this.conversationPresenter.Initialize();
            this.rewardPresenter.Initialize();
            this.onegaiPresenter.Initialize(playerOnegaiRepository);
            this.onegaiDetailPresenter.Initialize();
            this.rewardOnegaiPresenter.Initialize(playerOnegaiRepository);
            this.rewardMonoInfoPresenter.Initialize(monoInfoRepository);
            this.commonPresenter.Initialize();
            this.monoDetailPresenter.Initialize();
            this.mousePurchasePresenter.Initialize(mousePurchaceTableRepository, playerMouseStockRepository);
            this.toolBarmanager.Initialize();
            this.onegaiConditionNotificationPresenter.Initialize();
            this.imagePresenter.Initialize ();
            this.onegaiListModelPresenter.Initialize(playerOnegaiRepository);
            this.arrangementModeUIPresenter.Initialize();
            this.selectModeUIPresenter.Initialize();
            this.removeArrangementModeUIPresenter.Initialize();
            this.moveArrangementModeUIPresenter.Initialize();
        }

        public void UpdateByFrame() {
            this.toolBarmanager.UpdateByrame();
            this.onegaiConditionNotificationPresenter.UpdateByFrame();
            this.fieldActionUIPresenter.UpdateByFrame();
            this.removeArrangementModeUIPresenter.UpdateByFrame();
        }
    }
}