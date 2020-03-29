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
        private Camera mainCameta = null;

        // のちのち view として作れれば良い
        private List<GameObject> instances;

        // 設置のモード
        private IArrangementPresenterDeployMode arrangementPresenterDeployMode;

        private ArrangementPresenterDeployModeSetting arrangementPresenterDeployModeSetting;

        /// <summary>
        /// 配置物がタップされたときの
        /// </summary>
        public TypeObservable<IPlayerArrangementTarget> OnTouchArrangement { get; private set; }

        public void Initialize (IPlayerArrangementTargetRepository playerArrangementTargetRepository) {
            this.instances = new List<GameObject> ();
            this.OnTouchArrangement = new TypeObservable<IPlayerArrangementTarget>();
            this.arrangementPresenterDeployModeSetting = new ArrangementPresenterDeployModeSetting(
                this.mainCameta,
                this,
                this.gameObject,
                this.arrangementPrefab,
                this.selectedArrangementPrefab,
                this.reserveArrangementPrefab,
                playerArrangementTargetRepository
            );

            this.arrangementPresenterDeployMode = new ArrangementPresenterDeployMode(this.arrangementPresenterDeployModeSetting);
        }

        public void ReLoad () {

            this.ClearDisposable();

            foreach (var instance in this.instances) {
                Object.DisAppear (instance);
            }

            this.instances.Clear ();

            // 設置開始しているもの
            GameManager.Instance.ArrangementManager.ArrangementTargetStore.ForEach (arrangementTarget => {
                this.arrangementPresenterDeployMode.Arrangement(arrangementTarget);
            });
        }

        public void AddInstance(GameObject arrangementInstance)
        {
            this.instances.Add(arrangementInstance);
        }

        // 通常の配置モード
        public void ChangeToArrangementDeployMode()
        {
            if (this.arrangementPresenterDeployMode is ArrangementPresenterDeployMode) {
                return;
            }
            this.ChangeArrangementDeployMode(new ArrangementPresenterDeployMode(this.arrangementPresenterDeployModeSetting));
        }

        // 移動モード
        public void ChangeToMoveArrangementDeployMode()
        {
            if (this.arrangementPresenterDeployMode is MoveArrangementPresenterDeployMode) {
                return;
            }
            this.ChangeArrangementDeployMode(new MoveArrangementPresenterDeployMode(this.arrangementPresenterDeployModeSetting));
        }

        // 設置モードの変更
        private void ChangeArrangementDeployMode(IArrangementPresenterDeployMode arrangementPresenterDeployMode)
        {
            if (this.arrangementPresenterDeployMode != null) 
            {
                this.arrangementPresenterDeployMode.Dispose();
            }
            this.arrangementPresenterDeployMode = arrangementPresenterDeployMode;
        }
    }
}