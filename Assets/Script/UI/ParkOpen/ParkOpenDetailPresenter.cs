using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class ParkOpenDetailPresenter : UiWindowPresenterBase
    {
        [SerializeField]
        private ParkOpenDetailView parkOpenDetailView = null;

        private ParkOpenGroupModel currentSetParkOpenGroupModel = null;

        /// <summary>
        /// 出発が押された時
        /// </summary>
        /// <value></value>
        public TypeObservable<ParkOpenGroupModel> OnStartObservable { get; private set; }

        public void Initialize() {
            this.OnStartObservable = new TypeObservable<ParkOpenGroupModel>();
            this.parkOpenDetailView.Initialize();

            // 開始ボタンを押した場合
            this.disposables.Add(parkOpenDetailView.OnStartObservable.Subscribe(_ => {
                Debug.Assert(this.currentSetParkOpenGroupModel != null, "グループ情報がセットされていません");
                this.OnStartObservable.Execute(this.currentSetParkOpenGroupModel);                
            }));

            this.Close();
        }

        public void SetContents(ParkOpenGroupModel parkOpenGroupModel) {
            this.currentSetParkOpenGroupModel = parkOpenGroupModel;
            this.parkOpenDetailView.UpdateView(
                parkOpenGroupModel.ParkOpenGroupViewInfo.GroupName, 
                parkOpenGroupModel.ParkOpenGroupViewInfo.GroupDescription);
        }
    }    
}