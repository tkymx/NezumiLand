using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    /// <summary>
    /// 配置した際の配置していることがわかるオブジェクトの表示
    /// </summary>
    public class ArrangementPresenter : DisposableMonoBehaviour {

        [SerializeField]
        private GameObject arrangementPrefab = null;

        [SerializeField]
        private GameObject selectedArrangementPrefab = null;
        
        [SerializeField]
        private GameObject reserveArrangementPrefab = null;

        [SerializeField]
        private GameObject arrangementCancelAnnotationPrefab = null;

        [SerializeField]
        private Camera mainCameta = null;

        // のちのち view として作れれば良い
        private List<GameObject> instances;

        private UnReserveArrangementService unReserveArrangementService = null;

        /// <summary>
        /// 配置物がタップされたときの
        /// </summary>
        public TypeObservable<IPlayerArrangementTarget> OnTouchArrangement { get; private set; }

        public void Initialize (IPlayerArrangementTargetRepository playerArrangementTargetRepository) {
            this.instances = new List<GameObject> ();
            this.OnTouchArrangement = new TypeObservable<IPlayerArrangementTarget>();
            this.unReserveArrangementService = new UnReserveArrangementService(playerArrangementTargetRepository);
        }

        public void ReLoad () {

            this.ClearDisposable();

            foreach (var instance in this.instances) {
                Object.DisAppear (instance);
            }

            this.instances.Clear ();

            // 設置開始しているもの
            GameManager.Instance.ArrangementManager.ArrangementTargetStore.ForEach (arrangementTarget => {
                if (GameManager.Instance.ArrangementManager.CheckIsSelect (arrangementTarget)) 
                {
                    // 選択状態
                    this.appearArrangement (arrangementTarget, this.selectedArrangementPrefab);
                } 
                else if (arrangementTarget.ArrangementTargetState == ArrangementTargetState.Reserve) 
                {
                    // 予約状態
                    this.appearArrangement (arrangementTarget, reserveArrangementPrefab);
                    this.AddArrangementCancelAnnotation(arrangementTarget);
                } 
                else 
                {
                    // 設置状態
                    this.appearArrangement (arrangementTarget, this.arrangementPrefab);
                }
            });
        }

        private void AddArrangementCancelAnnotation (IPlayerArrangementTarget arrangementTarget) {
            var instance = Object.AppearToFloor (this.arrangementCancelAnnotationPrefab, this.gameObject, arrangementTarget.CenterPosition);
            var view = instance.GetComponent<ArrangementReserveCancelView>();
            view.Initialize(this.mainCameta);
            view.SetAnnnotationSize(arrangementTarget.MonoInfo.Width, arrangementTarget.MonoInfo.Height);
            this.disposables.Add(view.OnCancelObservable.Subscribe(_ => {
                this.unReserveArrangementService.Execute(arrangementTarget);
            }));
            this.disposables.Add(view.OnClickDirection.Subscribe(direction => {
                if(GameManager.Instance.ArrangementManager.MoveArrangement(arrangementTarget, direction))
                {
                    ReLoad();
                }
            }));

            this.instances.Add (instance);
        }

        private void appearArrangement (IPlayerArrangementTarget arrangementTarget, GameObject prefab, bool isReserve = false) {
            arrangementTarget.ArrangementPositions.ForEach (arrangementPosition => {
                var instance = Object.AppearToFloor (prefab, gameObject, new Vector3 (
                    arrangementPosition.x * ArrangementAnnotater.ArrangementWidth,
                    0,
                    arrangementPosition.z * ArrangementAnnotater.ArrangementHeight
                ));

                Debug.Assert (instance != null, "Arrangement が生成されていません");
                Debug.Assert (instance.GetComponent<ArrangementView> () != null, "Arrangement の ArrangementView が設定されておりません。");

                var arrangementView = instance.GetComponent<ArrangementView> ();

                // 選択時の挙動を追加
                if (!isReserve) {
                    arrangementView.OnSelect
                        .Subscribe (_ => {
                            this.OnTouchArrangement.Execute(arrangementTarget);
                        });
                }

                this.instances.Add (instance);
            });
        }
    }
}