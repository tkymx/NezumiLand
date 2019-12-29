using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NL {
    public class OnegaiMediater {
        private readonly IPlayerOnegaiRepository playerOnegaiRepository;

        public OnegaiMediater (IPlayerOnegaiRepository playerOnegaiRepository) {
            this.playerOnegaiRepository = playerOnegaiRepository;
        }
        
        /// <summary>
        /// 未クリアのお願いがクリアかを判断する
        /// </summary>
        /// <param name="conditionBase"></param>
        /// <param name="targetPlayerOnegaiModels"></param>
        public void Mediate (IOnegaiConditionBase conditionBase, List<PlayerOnegaiModel> targetPlayerOnegaiModels) {

            var mediatablePlayerOnegaiModels = targetPlayerOnegaiModels                
                .Where (model => model.OnegaiState == OnegaiState.UnLock)
                .Where (model => model.OnegaiModel.OnegaiCondition == conditionBase.OnegaiCondition)
                .ToList ();

            var clearPlayerOnegaiModels = conditionBase.Mediate (mediatablePlayerOnegaiModels);

            // クリアしたモデルを保存する
            foreach (var mediatablePlayerOnegaiModel in mediatablePlayerOnegaiModels) {
                var clearPlayerOnegaiModel = clearPlayerOnegaiModels.Find(model => model.Id == mediatablePlayerOnegaiModel.Id);

                // ないということは UnLock  であるということ
                if (clearPlayerOnegaiModel == null) {
                    continue;
                }

                // UnLock から Clear になったとき
                playerOnegaiRepository.Store (clearPlayerOnegaiModel);
                GameManager.Instance.EventManager.PushEventParameter(new NL.EventCondition.ClearOnegai(clearPlayerOnegaiModel.OnegaiModel));
                GameManager.Instance.GameUIManager.OnegaiConditionNotificationPresenter.PushNotification(clearPlayerOnegaiModel.OnegaiModel, OnegaiConditionNotificationState.Clear);
            }
        }

        /// <summary>
        /// すべてのお願いをリセットして、再度クリアかを判断する
        /// 条件が今も満たされているかをこれで判断する意図
        /// </summary>
        /// <param name="conditionBase"></param>
        /// <param name="targetPlayerOnegaiModels"></param>
        public void ClearResetAndMediate (IOnegaiConditionBase conditionBase, List<PlayerOnegaiModel> targetPlayerOnegaiModels) {

            var mediatablePlayerOnegaiModels = targetPlayerOnegaiModels                
                .Where (model => model.OnegaiState == OnegaiState.Clear)
                .Where (model => model.OnegaiModel.OnegaiCondition == conditionBase.OnegaiCondition)
                .ToList ();

            // クリア状況をリセットするモデルを保存する
            foreach (var mediatablePlayerOnegaiModel in mediatablePlayerOnegaiModels)
            {
                mediatablePlayerOnegaiModel.ToUnlock();
            }

            // クリア状況を更新する
            var clearPlayerOnegaiModels = conditionBase
                .Mediate (mediatablePlayerOnegaiModels)
                .ToList();

            // クリアからUnLockになっているものを更新
            foreach (var mediatablePlayerOnegaiModel in mediatablePlayerOnegaiModels) {
                var clearPlayerOnegaiModel = clearPlayerOnegaiModels.Find(model => model.Id == mediatablePlayerOnegaiModel.Id);

                // 存在するということはClearのママであるということ
                if (clearPlayerOnegaiModel != null) {
                    continue;
                }

                // Clear から UnLock のときの処理
                playerOnegaiRepository.Store (mediatablePlayerOnegaiModel);
                GameManager.Instance.GameUIManager.OnegaiConditionNotificationPresenter.PushNotification(mediatablePlayerOnegaiModel.OnegaiModel, OnegaiConditionNotificationState.Failue);
            }
        }

    }
}