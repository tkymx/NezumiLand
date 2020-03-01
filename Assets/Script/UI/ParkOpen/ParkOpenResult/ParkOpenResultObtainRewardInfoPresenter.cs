using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public class ParkOpenResultObtainRewardInfoPresenter : ListPresenterBase<ParkOpenResultAmount.SpecialRewardResult, ParkOpenResultObtainRewardInfoCellView> {
        protected override void onReloadCell (ParkOpenResultAmount.SpecialRewardResult element, ParkOpenResultObtainRewardInfoCellView cellView) {
            cellView.UpdateView(
                string.Format("{0}ハート獲得達成", element.ParkOpenRewardAmount.ObtainHeartCount),
                element.IsClear ? "成功" : "失敗"
            );
        }
    }
}
