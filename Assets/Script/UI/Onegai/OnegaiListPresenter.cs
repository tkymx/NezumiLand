using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class OnegaiListPresenter : ListPresenterBase<PlayerOnegaiModel, OnegaiListCellView>
    {
        protected override void onReloadCell(PlayerOnegaiModel element, OnegaiListCellView cellView)
        {
            cellView.UpdateCell(
                element.OnegaiModel.Title,
                element.OnegaiModel.Detail,
                element.OnegaiState == OnegaiState.Clear);
        }
    }
}
