using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public class ParkOpenStartRewardListPresenter : ListPresenterBase<IRewardAmount, ParkOpenStartRewardListView> {
        protected override void onReloadCell (IRewardAmount element, ParkOpenStartRewardListView cellView) {
            cellView.UpdateCell (
                element.Name,
                element.Image,
                element.Amount.ToString());
        }
    }
}