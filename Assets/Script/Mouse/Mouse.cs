using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    struct MouseParameter {
        public float speed;
    }

    public class Mouse : DisposableMonoBehaviour {
        [SerializeField]
        private SimpleAnimation simpleAnimation = null;

        /// <summary>
        /// ネズミの行動の状態遷移
        /// </summary>
        private StateManager stateManager;
        public StateManager StateManager => stateManager;

        private MouseParameter mouseParameter;

        private Vector3 moveVector;

        private PreMono currentPreMono;

        public PlayerMouseViewModel PlayerMouseViewModel { get; private set; }

        /// <summary>
        /// プレモノをっ持っているかどうか？
        /// </summary>
        public bool HasPreMono => currentPreMono != null;

        // Start is called before the first frame update
        void Awake () {
            // 初期化
            this.mouseParameter = new MouseParameter () {
                speed = 4.0f,
            };

            // 状態遷移
            this.stateManager = new StateManager (new EmptyState ());
            this.disposables.Add(this.stateManager.OnChangeStateObservable.Subscribe(state => {
                GameManager.Instance.MouseStockManager.ChangeState(this, state);
            }));

            this.currentPreMono = null;
        }

        // Update is called once per frame
        void Update () {
            InitializeByFrame ();
            stateManager.UpdateByFrame ();
            Move ();
            InfoUpdate();
        }

        private float infoUpdateTime;
        private const float InfoUpdateInterval = 0.5f;
        private void InfoUpdate () {
            this.infoUpdateTime += GameManager.Instance.TimeManager.DeltaTime ();
            if (this.infoUpdateTime > InfoUpdateInterval) {
                GameManager.Instance.MouseStockManager.ChangeTransform(this);
                if (this.currentPreMono != null) {
                    GameManager.Instance.MouseStockManager.ChangeMakingAmount(this, this.currentPreMono.CurrentMakingAmount);
                }
                this.infoUpdateTime = 0;
            }
        }

        void InitializeByFrame () {
            moveVector = Vector3.zero;
        }

        void Move () {
            if (moveVector.magnitude > 0.001) {
                simpleAnimation.CrossFade ("run", 0.5f);
                transform.rotation = Quaternion.LookRotation (moveVector);
                transform.position = transform.position + moveVector;
            } else {
                simpleAnimation.CrossFade ("idle", 0.5f);
            }
        }

        public void StartMake (PlayerArrangementTargetModel playerArrangementTargetModel) {
            if (!HasPreMono) {
                Debug.LogError ("プレモノを持っていないのにMakeが呼ばれました");
            }
            GameManager.Instance.MouseStockManager.ChangeTransform(this);
            currentPreMono.StartMaking (playerArrangementTargetModel);
        }

        public void ProgressMaking (MakingAmount deltaMakingAmount) {
            this.currentPreMono.ProgressMaking (deltaMakingAmount);
        }

        public bool IsFinishMaking () {
            return this.currentPreMono.IsFinishMaking ();
        }        

        public void FinishMaking (PlayerArrangementTargetModel playerArrangementTargetModel) {
            if (!HasPreMono) {
                Debug.LogError ("プレモノを持っていないのにMakeが呼ばれました");
            }
            currentPreMono.FinishMaking (playerArrangementTargetModel);
            currentPreMono = null;
        }

        // 移動のみを行う
        public void MoveTimeTo (Vector3 target) {
            moveVector = ObjectComparison.Direction (target, transform.position) * this.mouseParameter.speed * GameManager.Instance.TimeManager.DeltaTime ();
        }

        /// <summary>
        /// マウスにモノのオーダーする部分。実質ここが初期化
        /// </summary>
        /// <param name="arrangementTarget"></param>
        /// <param name="preMono"></param>
        public void OrderMaking (PlayerMouseViewModel playerMouseViewModel, PreMono preMono) {
            Debug.Assert (!IsOrdered (), "現在作成中のため追加で作成を行うことができません。");
            this.currentPreMono = preMono;
            this.PlayerMouseViewModel = playerMouseViewModel;
            stateManager.Interrupt (new MoveToTarget (this, playerMouseViewModel.PlayerArrangementTargetModel));
        }

        /// <summary>
        /// プレイヤーデータから再度マウスの状態を決める場合
        /// </summary>
        /// <param name="playerMouseViewModel"></param>
        /// <param name="preMono"></param>
        public void ReOrderMaking (PlayerMouseViewModel playerMouseViewModel, PreMono preMono) {
            Debug.Assert (!IsOrdered (), "現在作成中のため追加で作成を行うことができません。");

            this.currentPreMono = preMono;
            this.PlayerMouseViewModel = playerMouseViewModel;

            // 状態変化による切り替え
            if (playerMouseViewModel.State == MouseViewState.Move) {
                stateManager.Interrupt (new MoveToTarget (this, playerMouseViewModel.PlayerArrangementTargetModel));
            }
            else if (playerMouseViewModel.State == MouseViewState.Making) {
                stateManager.Interrupt (new MakingState (this, playerMouseViewModel.PlayerArrangementTargetModel));
                this.currentPreMono.ProgressMaking (playerMouseViewModel.MakingAmount); 
            }
            else if (playerMouseViewModel.State == MouseViewState.BackToHome) {
                stateManager.Interrupt (new BackToHomeState (this) );
            }
            else {
                Debug.Assert(false, "ありえない状態" + playerMouseViewModel.State.ToString());
            }
        }

        public bool IsOrdered () {
            return this.currentPreMono != null;
        }
    }
}