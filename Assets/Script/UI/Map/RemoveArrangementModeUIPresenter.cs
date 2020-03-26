using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class RemoveArrangementModeUIPresenter : UiWindowPresenterBase
    {
        [SerializeField]
        private RemoveArrangementModeUIView removeArrangementModeUIView = null;

        public TypeObservable<int> OnRemoveObservable { get; private set; }

        public void Initialize() {
            this.OnRemoveObservable = new TypeObservable<int>();
            this.removeArrangementModeUIView.Initialize();
            this.disposables.Add(removeArrangementModeUIView.OnBackObservable.Subscribe(_ => {
                this.Close();
            }));
            this.disposables.Add(removeArrangementModeUIView.OnRemoveObservable.Subscribe(_ => {
                this.OnRemoveObservable.Execute(0);
            }));
            this.Close();
        }

        public void UpdateByFrame()
        {
            // 現在のコストを取得
            var currentRemoveFee = ArrangementHelper.CalcAllSelectArrangmentRemoveFee();

            // 消費できるかで見た目を変える
            var isConsume = GameManager.Instance.Wallet.IsConsume(currentRemoveFee);
            this.removeArrangementModeUIView.UpdateView(currentRemoveFee.ToString(), !isConsume);
        }
    }    
}