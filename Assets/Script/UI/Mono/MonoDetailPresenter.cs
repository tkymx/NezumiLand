using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class MonoDetailPresenter : UiWindowPresenterBase
    {
        [SerializeField]
        private MonoDetailView monoDetailView = null;

        public void Initialize() {
            this.monoDetailView.Initialize();
            this.disposables.Add(this.monoDetailView.OnClose.Subscribe(_ => {
                this.Close();
            }));
            this.Close();
        }

        public void SetDetail(PlayerMonoInfo playerMonoInfo, int currentArrangementCount) {
            this.monoDetailView.UpdateView(
                playerMonoInfo.MonoInfo.Name,
                playerMonoInfo.MonoInfo.MakingFee.ToString(),
                playerMonoInfo.MonoInfo.ArrangementItemAmount.ToString(),
                currentArrangementCount.ToString(),
                playerMonoInfo.MonoInfo.ArrangementMaxCount.GetMaxCount(playerMonoInfo.MonoInfo.Id).ToString(),
                playerMonoInfo.MonoInfo.BaseSatisfaction.ToString(),
                playerMonoInfo.MonoInfo.Size()
            );
        }
    }   
}