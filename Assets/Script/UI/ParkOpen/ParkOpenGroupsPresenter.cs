using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace NL
{
    public class ParkOpenGroupsPresenter : UiWindowPresenterBase
    {
        [SerializeField]
        private ParkOpenGroupsView parkOpenGroupsView = null;

        [SerializeField]
        private GameObject parkOpenGroupCellRoot = null;

        [SerializeField]
        private GameObject parkOpenGroupCellPrefab = null;

        private Dictionary<ParkOpenGroupModel, ParkOpenGroupCellView> parkOpenGroupModelToCellViewDic = null;

        private Sprite defaultFrame = null;

        private List<IDisposable> cellDisposables = new List<IDisposable>();

        /// <summary>
        /// 遊び場開放をしたときのオブザーバブル
        /// </summary>
        /// <value></value>
        public TypeObservable<ParkOpenGroupModel> OnStartParkOpenGroupObservable { get; private set; }

        public void Initialize() {
            this.OnStartParkOpenGroupObservable = new TypeObservable<ParkOpenGroupModel>();
            this.parkOpenGroupModelToCellViewDic = new Dictionary<ParkOpenGroupModel, ParkOpenGroupCellView>();

            this.parkOpenGroupsView.Initialize();
            this.disposables.Add(parkOpenGroupsView.OnBackObservable.Subscribe(_ => {
                this.Close();
            }));
            this.Close();

            this.defaultFrame = ResourceLoader.LoadParkOpenGroupIconSprite("frame");
        }

        public void SetContents(ParkOpenGroupsModel parkOpenGroupsModel)
        {
            // 寄贈の要素を消去
            foreach (Transform child in parkOpenGroupCellRoot.transform) {
                Object.DisAppear (child.gameObject);
            }

            this.parkOpenGroupModelToCellViewDic.Clear ();

            // disposable をクリアする
            foreach (var cellDisposable in this.cellDisposables)
            {
                cellDisposable.Dispose();
            }
            this.cellDisposables.Clear();

            // セルの生成
            foreach (var parkOpenGroup in parkOpenGroupsModel.ParkOpenGroupModels)
            {
                var instance = Object.AppearLocal2D(this.parkOpenGroupCellPrefab, this.parkOpenGroupCellRoot, parkOpenGroup.ParkOpenGroupViewInfo.SelectorPosition);
                var parkOpenGroupCellView = instance.GetComponent<ParkOpenGroupCellView>();
                Debug.Assert(parkOpenGroupCellView != null, "ParkOpenGroupCellViewが存在しません");

                // 初期化
                parkOpenGroupCellView.Initialize();

                // 情報を設定
                var icon = ResourceLoader.LoadParkOpenGroupIconSprite(parkOpenGroup.ParkOpenGroupViewInfo.IconName);
                parkOpenGroupCellView.UpdateView(defaultFrame, icon, false /*後でプレイヤーデータから取得*/);

                // イベントの設定
                this.cellDisposables.Add(parkOpenGroupCellView.OnSelectObservable.Subscribe(_ => {
                    this.Select(parkOpenGroup);

                    GameManager.Instance.GameUIManager.ParkOpenDetailPresenter.Show();
                    GameManager.Instance.GameUIManager.ParkOpenDetailPresenter.SetContents(parkOpenGroup);
                }));

                // 追加
                this.parkOpenGroupModelToCellViewDic.Add(parkOpenGroup, parkOpenGroupCellView);
            }

            // スタートのオブザーバブル
            this.cellDisposables.Add(GameManager.Instance.GameUIManager.ParkOpenDetailPresenter.OnStartObservable.Subscribe(parkOpenGroup => {
                this.OnStartParkOpenGroupObservable.Execute(parkOpenGroup);
            }));

            // 選択解除
            this.Select(null);
        }

        public override void onPrepareClose()
        {
            GameManager.Instance.GameUIManager.ParkOpenDetailPresenter.Close();
        }

        // グループの選択を行う
        private void Select(ParkOpenGroupModel parkOpenGroupModel)
        {
            foreach (var keyValue in this.parkOpenGroupModelToCellViewDic)
            {
                keyValue.Value.SetSelectVisible(keyValue.Key.Equals(parkOpenGroupModel));
            }
        }
    }    
}