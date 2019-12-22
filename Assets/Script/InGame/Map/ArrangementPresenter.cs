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
        private List<ArrangementView> arrangementViews;

        public void Initialize () {
            this.arrangementViews = new List<ArrangementView> ();
        }

        public void ReLoad () {

            this.ClearDisposable();

            foreach (var arrangementView in this.arrangementViews) {
                Object.DisAppear (arrangementView.gameObject);
            }

            this.arrangementViews.Clear ();

            // 設置開始しているもの
            GameManager.Instance.ArrangementManager.ArrangementTargetStore.ForEach (arrangementTarget => {
                if (GameManager.Instance.ArrangementManager.CheckIsSelect (arrangementTarget)) 
                {
                    this.appearArrangement (arrangementTarget, this.selectedArrangementPrefab);
                } 
                else if (arrangementTarget.ArrangementTargetState == ArrangementTargetState.Reserve) 
                {
                    this.appearArrangement (arrangementTarget, reserveArrangementPrefab);
                    this.AddArrangementCancelAnnotation(arrangementTarget);

                } 
                else 
                {
                    this.appearArrangement (arrangementTarget, this.arrangementPrefab);
                }
            });
        }

        private void AddArrangementCancelAnnotation (IArrangementTarget arrangementTarget) {
            var instance = Object.AppearToFloor (this.arrangementCancelAnnotationPrefab, this.gameObject, arrangementTarget.CenterPosition);
            var view = instance.GetComponent<ArrangementReserveCancelView>();
            view.Initialize(this.mainCameta);
            this.disposables.Add(view.OnCancelObservable.Subscribe(_ => {
                UnReserveArrangementService.Execute(arrangementTarget);
            }));
        }

        private void appearArrangement (IArrangementTarget arrangementTarget, GameObject prefab, bool isReserve = false) {
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
                            // メニューを変更する
                            GameManager.Instance.GameModeManager.EnqueueChangeMode (GameModeGenerator.GenerateArrangementMenuSelectMode (arrangementTarget));
                        });
                }

                this.arrangementViews.Add (arrangementView);
            });
        }
    }
}