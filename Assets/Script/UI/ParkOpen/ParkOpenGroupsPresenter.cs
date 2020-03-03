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

        private IPlayerParkOpenGroupRepository playerParkOpenGroupRepository = null;

        /// <summary>
        /// 遊び場開放をしたときのオブザーバブル
        /// </summary>
        /// <value></value>
        public TypeObservable<PlayerParkOpenGroupModel> OnStartParkOpenGroupObservable { get; private set; }

        public void Initialize(IPlayerParkOpenGroupRepository playerParkOpenGroupRepository) {

            this.playerParkOpenGroupRepository = playerParkOpenGroupRepository;
            this.OnStartParkOpenGroupObservable = new TypeObservable<PlayerParkOpenGroupModel>();
            this.parkOpenGroupModelToCellViewDic = new Dictionary<ParkOpenGroupModel, ParkOpenGroupCellView>();

            this.parkOpenGroupsView.Initialize();
            
            // 選択をキャンセルする
            this.disposables.Add(parkOpenGroupsView.OnSelectCancelObservable.Subscribe(_ => {
                this.Select(null);
            }));

            this.defaultFrame = ResourceLoader.LoadParkOpenGroupIconSprite("frame");
        }

        public void SetContents(ParkOpenGroupsModel parkOpenGroupsModel)
        {
            // 既存の要素を消去
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
            var playerParkOpenGroups = this.playerParkOpenGroupRepository.GetDisplayale(parkOpenGroupsModel);
            foreach (var playerParkOpenGroup in playerParkOpenGroups)
            {
                var instance = Object.AppearLocal2D(this.parkOpenGroupCellPrefab, this.parkOpenGroupCellRoot, playerParkOpenGroup.ParkOpenGroupModel.ParkOpenGroupViewInfo.SelectorPosition);
                var parkOpenGroupCellView = instance.GetComponent<ParkOpenGroupCellView>();
                Debug.Assert(parkOpenGroupCellView != null, "ParkOpenGroupCellViewが存在しません");

                // 初期化
                parkOpenGroupCellView.Initialize();

                // 情報を設定
                var icon = ResourceLoader.LoadParkOpenGroupIconSprite(playerParkOpenGroup.ParkOpenGroupModel.ParkOpenGroupViewInfo.IconName);
                parkOpenGroupCellView.UpdateView(defaultFrame, icon, playerParkOpenGroup.IsClear, playerParkOpenGroup.IsSpecial);

                // イベントの設定
                this.cellDisposables.Add(parkOpenGroupCellView.OnSelectObservable.Subscribe(_ => {
                    this.Select(playerParkOpenGroup.ParkOpenGroupModel);

                    GameManager.Instance.GameUIManager.ParkOpenDetailPresenter.Show();
                    GameManager.Instance.GameUIManager.ParkOpenDetailPresenter.SetContents(playerParkOpenGroup);
                }));

                // 追加
                this.parkOpenGroupModelToCellViewDic.Add(playerParkOpenGroup.ParkOpenGroupModel, parkOpenGroupCellView);
            }

            // スタートのオブザーバブル
            this.cellDisposables.Add(GameManager.Instance.GameUIManager.ParkOpenDetailPresenter.OnStartObservable.Subscribe(playerParkOpenGroup => {
                this.OnStartParkOpenGroupObservable.Execute(playerParkOpenGroup);
            }));

            // 背景を変更
            var backgourndImage = ResourceLoader.LoadParkParkOpenGroupBackgound(parkOpenGroupsModel.BackgroundSpriteName);
            this.parkOpenGroupsView.UpdateView(backgourndImage);

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
            if (parkOpenGroupModel == null) {
                GameManager.Instance.GameUIManager.ParkOpenDetailPresenter.Close();                
            }
            
            foreach (var keyValue in this.parkOpenGroupModelToCellViewDic)
            {
                keyValue.Value.SetSelectVisible(keyValue.Key.Equals(parkOpenGroupModel));
            }
        }
    }    
}