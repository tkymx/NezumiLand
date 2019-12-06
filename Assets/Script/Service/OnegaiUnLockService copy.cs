using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  NL
{
    public class OnegaiUnLockService
    {
        private readonly OnegaiRepository onegaiRepository = null;
        private readonly IPlayerOnegaiRepository playerOnegaiRepository = null;

        public OnegaiUnLockService(OnegaiRepository onegaiRepository, IPlayerOnegaiRepository playerOnegaiRepository )
        {
            this.onegaiRepository = onegaiRepository;
            this.playerOnegaiRepository = playerOnegaiRepository;
        }
        public void Execute() 
        {
            foreach (var onegaiModel in this.onegaiRepository.GetAll ()) {

                // モデルを取得
                var playerOnegaiModel = playerOnegaiRepository.GetById(onegaiModel.Id);
                if (!playerOnegaiModel.IsLock()) {
                    continue;
                }

                // アンロックにする
                playerOnegaiModel.ToUnlock();

                // お願いをキャッシュする
                GameManager.Instance.OnegaiMediaterManager.ChacheOnegai(playerOnegaiModel.OnegaiModel);

                // 保存する
                this.playerOnegaiRepository.Store(playerOnegaiModel);
            }
        }
    }   
}