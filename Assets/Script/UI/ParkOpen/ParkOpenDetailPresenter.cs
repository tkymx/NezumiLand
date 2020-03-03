using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class ParkOpenDetailPresenter : UiWindowPresenterBase
    {
        [SerializeField]
        private ParkOpenDetailView parkOpenDetailView = null;

        private PlayerParkOpenGroupModel currentSetPlayerParkOpenGroupModel = null;

        /// <summary>
        /// 出発が押された時
        /// </summary>
        /// <value></value>
        public TypeObservable<PlayerParkOpenGroupModel> OnStartObservable { get; private set; }

        public void Initialize() {
            this.OnStartObservable = new TypeObservable<PlayerParkOpenGroupModel>();
            this.parkOpenDetailView.Initialize();

            // 開始ボタンを押した場合
            this.disposables.Add(parkOpenDetailView.OnStartObservable.Subscribe(_ => {
                Debug.Assert(this.currentSetPlayerParkOpenGroupModel != null, "グループ情報がセットされていません");
                this.OnStartObservable.Execute(this.currentSetPlayerParkOpenGroupModel);                
            }));

            this.Close();
        }

        public void SetContents(PlayerParkOpenGroupModel playerParkOpenGroupModel) {
            this.currentSetPlayerParkOpenGroupModel = playerParkOpenGroupModel;
            this.parkOpenDetailView.UpdateView(
                playerParkOpenGroupModel.ParkOpenGroupModel.ParkOpenGroupViewInfo.GroupName, 
                playerParkOpenGroupModel.ParkOpenGroupModel.ParkOpenGroupViewInfo.GroupDescription,
                playerParkOpenGroupModel.ParkOpenGroupModel.GoalHeartCount.ToString(),
                playerParkOpenGroupModel.IsSpecial);
        }
    }    
}