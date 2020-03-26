using System;
using System.Linq;

namespace NL
{
    public class ArrangementHelper
    {
        public static Currency CalcAllSelectArrangmentRemoveFee()
        {
            Currency currency = new Currency();
            foreach (var arrangementTarget in GameManager.Instance.ArrangementManager.SelectedArrangementTargets)
            {
                currency += arrangementTarget.MonoInfo.RemoveFee;
            }
            return currency;
        }
        public static ArrangementItemAmount CalcAllSelectArrangmentItemAmount()
        {
            ArrangementItemAmount arrangementItemAmount = new ArrangementItemAmount();
            foreach (var arrangementTarget in GameManager.Instance.ArrangementManager.SelectedArrangementTargets)
            {
                arrangementItemAmount += arrangementTarget.MonoInfo.ArrangementItemAmount;
            }
            return arrangementItemAmount;
        }
    }
}