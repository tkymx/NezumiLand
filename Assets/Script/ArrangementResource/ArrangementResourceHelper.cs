using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    /// <summary>
    /// 配置するときのアイテムの管理を行う
    /// </summary>
    public class ArrangementResourceHelper {

        public enum ErrorReason {
            UnKnown,
            NoCurrency,
            NoArrangementItem,
            NoMouse,
            NoArrangement,
        }

        public struct IsConsumeResult {
            public bool IsConsume { get; private set; }
            public string SystemErrorMessage { get; private set; }
            public ErrorReason ErrorReason { get; private set; }

            public string GetErrorMessage () {
                switch (this.ErrorReason)
                {
                    case ErrorReason.NoMouse: {
                        return "配置できるねずみがいません";
                    }
                    case ErrorReason.NoCurrency: {
                        return "お金がたりません";
                    }
                    case ErrorReason.NoArrangementItem: {
                        return "素材がたりません";
                    }
                    case ErrorReason.NoArrangement: {
                        return "最大配置数を超えています";
                    }
                    default: {
                        break;
                    }
                }
                return "設置できませんでした。";
            }

            public IsConsumeResult (bool isConsume, string error, ErrorReason errorReason) {
                this.IsConsume = isConsume;
                this.SystemErrorMessage = error;
                this.ErrorReason = errorReason;
            }
        }

        public static IsConsumeResult IsConsume (ArrangementResourceAmount amount, bool withReserve = true) {
            
            if (!GameManager.Instance.MouseStockManager.IsConsume (amount.MouseOrderAmount, withReserve) ) {
                return new IsConsumeResult(false, string.Format("MouseOrderAmount:{0}/{1}", amount.MouseOrderAmount, GameManager.Instance.MouseStockManager.MouseStockCount), ErrorReason.NoMouse);
            }
            
            if (!GameManager.Instance.Wallet.IsConsume (amount.Currency, withReserve)) {
                return new IsConsumeResult(false, string.Format("Currency:{0}/{1}", amount.Currency, GameManager.Instance.Wallet.Current), ErrorReason.NoCurrency);
            }

            if (!GameManager.Instance.ArrangementItemStore.IsConsume (amount.ArrangementItemAmount, withReserve)) {
                return new IsConsumeResult(false, string.Format("ArrangementItemStore:{0}/{1}", amount.ArrangementItemAmount, GameManager.Instance.ArrangementItemStore.Current), ErrorReason.NoArrangementItem);
            }

            var monoIds = amount.ArrangementCount.GetCountedMonoInfos();
            var monoRepository = new MonoInfoRepository(ContextMap.DefaultMap);
            var arrangementMaxCount = monoRepository.GetMaxCount(monoIds);
            foreach (var monoId in monoIds)
            {
                var currentCount = GameManager.Instance.ArrangementManager.GetAppearMonoCountById(monoId, withReserve);
                var maxCount = arrangementMaxCount.GetMaxCount(monoId);
                if (maxCount <= currentCount) {
                    return new IsConsumeResult(false, string.Format("currentCount:{0}:{1}/{2}", monoId, currentCount, maxCount), ErrorReason.NoArrangement);
                }
            }
            
            return new IsConsumeResult(true, "success", ErrorReason.UnKnown);
        }

        public static void Consume (ArrangementResourceAmount amount) {
            var isConsumeResult = ArrangementResourceHelper.IsConsume (amount, false);
            Debug.Assert (isConsumeResult.IsConsume, isConsumeResult.SystemErrorMessage);
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