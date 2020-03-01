using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NL
{
    public abstract class ParkOpenStartRewardListCellViewBase : ListCellViewBase
    {
        public abstract void UpdateView(IParkOpenCellElement parkOpenCellElement);
    }   
}
