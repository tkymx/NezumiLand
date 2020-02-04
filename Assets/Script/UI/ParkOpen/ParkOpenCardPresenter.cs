using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class ParkOpenCardPresenter : UiWindowPresenterBase
    {
        [SerializeField]
        private ParkOpenCardView ParkOpenCardView = null;
        
        public TypeObservable<int> OnTouchCardObservable { get; private set; }
        public TypeObservable<int> OnCancelObservable { get; private set; }
        public TypeObservable<int> OnUseObservable { get; private set; }

        private bool isPrepareState = false;

        public void Initialize() {
            this.ParkOpenCardView.Initialize();

            this.OnTouchCardObservable = new TypeObservable<int>();
            this.OnCancelObservable = new TypeObservable<int>();
            this.OnUseObservable = new TypeObservable<int>();
            this.disposables.Add(ParkOpenCardView.OnTouchCardObservable.Subscribe(_ => {
                this.OnTouchCardObservable.Execute(_);
            }));
            this.disposables.Add(ParkOpenCardView.OnCancelObservable.Subscribe(_ => {
                this.OnCancelObservable.Execute(_);
            }));
            this.disposables.Add(ParkOpenCardView.OnUseObservable.Subscribe(_ => {
                this.OnUseObservable.Execute(_);
            }));

            this.Close();
            this.isPrepareState = false;
        }

        public void TogglePrepare()
        {
            if (this.isPrepareState) {
                this.CancelPrepare();
            }
            else {
                this.PrepareUsing();
            }
        }

        public void PrepareUsing()
        {
            if (this.isPrepareState) {
                return;
            }

            this.ParkOpenCardView.PlayAnimation(ParkOpenCardView.AnimationTag.ToLarge);
            this.ParkOpenCardView.SetPrepareButtonVisible(true);
            this.ParkOpenCardView.transform.SetAsLastSibling();
            this.isPrepareState = true;
        }

        public void CancelPrepare()
        {
            if (!this.isPrepareState) {
                return;
            }

            this.ParkOpenCardView.PlayAnimation(ParkOpenCardView.AnimationTag.ToSmall);
            this.ParkOpenCardView.SetPrepareButtonVisible(false);
            this.isPrepareState = false;
        }

        public void SetContents(ParkOpenCardModel parkOpenCardModel) {
            var sprite = ResourceLoader.LoadCardSprite(parkOpenCardModel.ImageName);
            this.ParkOpenCardView.UpdateView(parkOpenCardModel.Name, parkOpenCardModel.Description, sprite);            
        }

        public void UseCard()
        {
            this.ParkOpenCardView.gameObject.SetActive(false);
        }

        public void ResetCard()
        {
            this.ParkOpenCardView.gameObject.SetActive(true);
        }

        public override void onPrepareShow()
        {
            this.isPrepareState = false;
            this.ParkOpenCardView.SetPrepareButtonVisible(false);
            this.ParkOpenCardView.PlayAnimation(ParkOpenCardView.AnimationTag.Initial);
        }
    }    
}