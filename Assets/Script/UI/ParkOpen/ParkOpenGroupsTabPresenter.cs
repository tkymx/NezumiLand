using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace NL {
    public class ParkOpenGroupsTabPresenter : UiWindowPresenterBase {

        private IParkOpenGroupsRepository parkOpenGroupsRepository;

        [SerializeField]
        private ParkOpenGroupsTabView parkOpenGroupsTabView = null;
        
        [SerializeField]
        private ParkOpenGroupsPresenter parkOpenGroupsPresenter = null;

        /// <summary>
        /// 遊び場開放をしたときのオブザーバブル
        /// </summary>
        /// <value></value>
        public TypeObservable<ParkOpenGroupModel> OnStartParkOpenGroupObservable { get; private set; }

        public void Initialize (IParkOpenGroupsRepository parkOpenGroupsRepository) {
            this.OnStartParkOpenGroupObservable = new TypeObservable<ParkOpenGroupModel>();
            this.parkOpenGroupsRepository = parkOpenGroupsRepository;

            this.parkOpenGroupsTabView.Initialize();
            this.parkOpenGroupsPresenter.Initialize();

            // タブを押された時
            this.disposables.Add(this.parkOpenGroupsTabView.OnClickTypeObservable.Subscribe(type => {
                this.SelectTab(type);
            }));

            // バックを押された時
            this.disposables.Add(this.parkOpenGroupsTabView.OnBackObservable.Subscribe(_ => {
                GameManager.Instance.GameModeManager.EnqueueChangeMode (GameModeGenerator.GenerateSelectMode ());
                this.Close ();
            }));

            // 選択された時
            this.disposables.Add(this.parkOpenGroupsPresenter.OnStartParkOpenGroupObservable.Subscribe(parkOpenGroupModel => {
                this.OnStartParkOpenGroupObservable.Execute(parkOpenGroupModel);
            }));

            this.Close ();
        }

        private void SelectTab (ParkOpenGroupsType parkOpenGroupsType) {
            this.parkOpenGroupsTabView.UpdateButtonEnabled(parkOpenGroupsType);

            // 表示可能なモノを取得
            var parkOpenGroupsModel = this.parkOpenGroupsRepository.GetByType(parkOpenGroupsType);
            this.parkOpenGroupsPresenter.SetContents(parkOpenGroupsModel);
        }

        public override void onPrepareShow () {
            base.onPrepareShow ();
            this.SelectTab (ParkOpenGroupsType.Story);
        }

        public override void onPrepareClose() {
            base.onPrepareClose();
            GameManager.Instance.GameUIManager.ParkOpenDetailPresenter.Close();
        }
    }
}