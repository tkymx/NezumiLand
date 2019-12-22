using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    /// <summary>
    /// 配置するときのアイテムの管理を行う
    /// </summary>
    public class ArrangementResourceHelper {
        public static bool IsConsume (ArrangementResourceAmount amount, bool withReserve = true) {
            
            if (!GameManager.Instance.MouseStockManager.IsConsume (amount.MouseOrderAmount, withReserve) ) {
                return false;
            }
            
            if (!GameManager.Instance.Wallet.IsConsume (amount.Currency, withReserve)) {
                return false;
            }

            if (!GameManager.Instance.ArrangementItemStore.IsConsume (amount.ArrangementItemAmount)) {
                return false;
            }

            var monoIds = amount.ArrangementCount.GetCountedMonoInfos();
            var monoRepository = new MonoInfoRepository(ContextMap.DefaultMap);
            var arrangementMaxCount = monoRepository.GetMaxCount(monoIds);
            foreach (var monoId in monoIds)
            {
                var currentCount = GameManager.Instance.ArrangementManager.GetAppearMonoCountById(monoId, withReserve);
                var maxCount = arrangementMaxCount.GetMaxCount(monoId);
                if (maxCount <= currentCount) {
                    return false;
                }
            }
            
            return true;
        }

        public static void Consume (ArrangementResourceAmount amount) {
            Debug.Assert (ArrangementResourceHelper.IsConsume (amount, false), "精算ができません");
            GameManager.Instance.Wallet.Consume (amount.Currency);
            GameManager.Instance.ArrangementItemStore.Consume (amount.ArrangementItemAmount);
        }

        private const string ReserveKey = "ArrangementResourceHelper";

        public static void ResetReserve () {
            GameManager.Instance.ReserveAmountManager.Reset(ReserveKey);
        }

        public static void ReserveConsume (ArrangementResourceAmount amount) {
            GameManager.Instance.ReserveAmountManager.Reserve<Currency>(ReserveKey, amount.Currency);
            GameManager.Instance.ReserveAmountManager.Reserve<ArrangementItemAmount>(ReserveKey, amount.ArrangementItemAmount);
            GameManager.Instance.ReserveAmountManager.Reserve<ArrangementCount>(ReserveKey, amount.ArrangementCount);
            GameManager.Instance.ReserveAmountManager.Reserve<MouseOrderAmount>(ReserveKey, amount.MouseOrderAmount);
        }

        public static void UnReserveConsume (ArrangementResourceAmount amount) {
            GameManager.Instance.ReserveAmountManager.UnReserve<Currency>(ReserveKey, amount.Currency);
            GameManager.Instance.ReserveAmountManager.UnReserve<ArrangementItemAmount>(ReserveKey, amount.ArrangementItemAmount);
            GameManager.Instance.ReserveAmountManager.UnReserve<ArrangementCount>(ReserveKey, amount.ArrangementCount);
            GameManager.Instance.ReserveAmountManager.UnReserve<MouseOrderAmount>(ReserveKey, amount.MouseOrderAmount);
        }

        public static void ConsumeReserve () {
            var amount = new ArrangementResourceAmount(
                GameManager.Instance.ReserveAmountManager.GetByKey<Currency>(ReserveKey),
                GameManager.Instance.ReserveAmountManager.GetByKey<ArrangementItemAmount>(ReserveKey),
                GameManager.Instance.ReserveAmountManager.GetByKey<ArrangementCount>(ReserveKey),
                GameManager.Instance.ReserveAmountManager.GetByKey<MouseOrderAmount>(ReserveKey));

            GameManager.Instance.EffectManager.PlayConsumeEffect (
                amount.Currency, 
                GameManager.Instance.MouseHomeManager.HomePostion);

            Consume(amount);
            ResetReserve();
        }
    }
}