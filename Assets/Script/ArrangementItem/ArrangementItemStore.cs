using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public class ArrangementItemStore : ConsumableCollectionBase<ArrangementItemAmount>{

        private UpdateArrangementItemService updateArrangementItemService = null;

        private ArrangementItemAmount current;
        public override ArrangementItemAmount Current => current;
        public override ArrangementItemAmount CurrentWithReserve => current - GameManager.Instance.ReserveAmountManager.Get<ArrangementItemAmount>();

        public ArrangementItemStore (ArrangementItemAmount arrangementItemAmount, IPlayerInfoRepository playerInfoRepository) {
            this.current = arrangementItemAmount;
            this.updateArrangementItemService = new UpdateArrangementItemService(playerInfoRepository);
        }

        public void ForceSet (ArrangementItemAmount arrangementItemAmount) {
            this.current = arrangementItemAmount;
        }

        public override bool OnIsConsume (ArrangementItemAmount amount) {
            return this.current >= amount;
        }

        public override void OnConsume (ArrangementItemAmount amount) {
            Debug.Assert (IsConsume (amount), "消費することができません");
            this.current -= amount;
            this.updateArrangementItemService.Execute(this.current);
        }

        public void Push (ArrangementItemAmount amount) {
            this.current += amount;
            this.updateArrangementItemService.Execute(this.current);
        }
    }
}