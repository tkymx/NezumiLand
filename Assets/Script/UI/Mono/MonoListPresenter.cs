using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public class MonoListPresenter : ListPresenterBase<PlayerMonoInfo, MonoListCellView> {
        protected override void onReloadCell (PlayerMonoInfo element, MonoListCellView cellView) {
            cellView.UpdateCell (
                element.MonoInfo.Name,
                element.MonoInfo.MakingFee,
                GameManager.Instance.ArrangementManager.GetAppearMonoCountById (element.MonoInfo.Id),
                element.MonoInfo.ArrangementCount.Count,
                !element.IsRelease);

            this.disposables.Add(cellView.OnClick.Subscribe (_ => {
                GameManager.Instance.MonoSelectManager.SelectMonoInfo (element.MonoInfo);
            }));
            this.disposables.Add(cellView.OnClickDetail.Subscribe(_=>{
                GameManager.Instance.GameUIManager.MonoDetailPresenter.SetDetail(element, GameManager.Instance.ArrangementManager.GetAppearMonoCountById (element.MonoInfo.Id));
                GameManager.Instance.GameUIManager.MonoDetailPresenter.Show();
            }));
            this.disposables.Add(cellView.OnClickLock.Subscribe(_=>{
                GameManager.Instance.GameUIManager.CommonPresenter.SetContents(Thesaurus.NotifyReleaseConditionModalTitle, element.MonoInfo.ReleaseConditionText);
                GameManager.Instance.GameUIManager.CommonPresenter.Show();
            }));
        }

        private void Update () {
            foreach (var pair in this.displayElementCellDictionary) {
                var playerMonoInfo = pair.Key;
                var cellView = pair.Value;

                if (this.IsEnable(playerMonoInfo)) {
                    cellView.Enable ();
                } else {
                    cellView.Diasble ();
                }
            }
        }

        private bool IsEnable(PlayerMonoInfo playerMonoInfo) {
            if (!playerMonoInfo.IsRelease) {
                return true;
            }
            if (!ArrangementResourceHelper.IsConsume (playerMonoInfo.MonoInfo.ArrangementResourceAmount)) {
                return false;
            }
            return true;
        }
    }
}