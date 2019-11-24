using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public class MonoListPresenter : ListPresenterBase<MonoInfo, MonoListCellView> {
        protected override void onReloadCell (MonoInfo element, MonoListCellView cellView) {
            cellView.UpdateCell (
                element.Name,
                element.MakingFee,
                GameManager.Instance.ArrangementManager.GetAppearMonoCountById (element.Id),
                element.ArrangementCount.Count);

            cellView.OnClick
                .Subscribe (_ => {
                    GameManager.Instance.MonoSelectManager.SelectMonoInfo (element);
                });
        }

        private void Update () {
            foreach (var pair in this.displayElementCellDictionary) {
                var monoInfo = pair.Key;
                var cellView = pair.Value;

                if (!ArrangementResourceHelper.IsConsume (monoInfo.ArrangementResourceAmount)) {
                    cellView.DiasbleForLowFee ();
                } else {
                    cellView.Enable ();
                }
            }
        }
    }
}