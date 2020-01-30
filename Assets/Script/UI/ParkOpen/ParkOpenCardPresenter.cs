using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class ParkOpenCardPresenter : UiWindowPresenterBase
    {
        [SerializeField]
        private ParkOpenCardView ParkOpenCardView = null;

        public void Initialize() {
            this.ParkOpenCardView.Initialize();
            this.disposables.Add(ParkOpenCardView.OnTouchCardObservable.Subscribe(_ => {
                //TODO: タッチしたときの挙動を追加
            }));
            this.Close();
        }

        public void SetContents(ParkOpenCardModel parkOpenCardModel) {
            var sprite = ResourceLoader.LoadCardSprite(parkOpenCardModel.ImageName);
            this.ParkOpenCardView.UpdateView(parkOpenCardModel.Name, parkOpenCardModel.Description, sprite);            
        }
    }    
}