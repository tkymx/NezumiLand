using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace NL {
    public class AppearArrangementService
    {
        public static void Execute () {
            ArrangementResourceHelper.ConsumeReserve();

            var reserveArrangementTarget = GameManager.Instance.ArrangementManager.ArrangementTargetStore
                .Where(target => target.ArrangementTargetState == ArrangementTargetState.Reserve)
                .ToList();

            foreach (var arrangementTarget in reserveArrangementTarget) {
                arrangementTarget.ToAppear();
                GameManager.Instance.MouseStockManager.OrderMouse (arrangementTarget);
            }

            GameManager.Instance.ArrangementPresenter.ReLoad ();
        }
    }
}

