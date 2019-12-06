using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  NL
{
    public class OnegaiUnLockChacheService
    {
        private readonly IPlayerOnegaiRepository playerOnegaiRepository = null;

        public OnegaiUnLockChacheService(IPlayerOnegaiRepository playerOnegaiRepository )
        {
            this.playerOnegaiRepository = playerOnegaiRepository;
        }
        public void Execute() 
        {
            foreach (var playerOnegaiModel in this.playerOnegaiRepository.GetAll ()) {

                // モデルを取得
                if (playerOnegaiModel.IsLock()) {
                    continue;
                }

                // お願いをキャッシュする
                GameManager.Instance.OnegaiMediaterManager.ChacheOnegai(playerOnegaiModel.OnegaiModel);
            }
        }
    }   
}