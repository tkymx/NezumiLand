using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  NL
{
    public class OnegaiManager
    {
        private readonly IPlayerOnegaiRepository playerOnegaiRepository;

        private Queue<uint> reserveUnLockOnegaiIdQueue = null;

        public OnegaiManager (IPlayerOnegaiRepository playerOnegaiRepository) {
            this.playerOnegaiRepository = playerOnegaiRepository;
            this.reserveUnLockOnegaiIdQueue = new Queue<uint>();
        }

        public void EnqueueUnLockReserve(uint onegaiId) {
            this.reserveUnLockOnegaiIdQueue.Enqueue(onegaiId);
        }

        public void UpdateByFrame() {

            // UnLock 予約を UnLock にする。
            Debug.Assert(this.reserveUnLockOnegaiIdQueue != null, "reserveUnLockQueue が null です");
            while(this.reserveUnLockOnegaiIdQueue.Count > 0) {

                // プレイヤーデータを取得
                var onegaiId = this.reserveUnLockOnegaiIdQueue.Dequeue();
                var playerOnegaiModel = this.playerOnegaiRepository.GetById(onegaiId);
                Debug.Assert(playerOnegaiModel != null, "playerOnegaiModel が null です");
                
                // アンロックにする
                Debug.Assert(playerOnegaiModel.IsLock(), playerOnegaiModel.Id.ToString() + "がLock状態ではありません");
                playerOnegaiModel.ToUnlock();

                // キャッシュに入れる
                GameManager.Instance.OnegaiMediaterManager.ChacheOnegai(playerOnegaiModel.OnegaiModel);
                
                // 保存する
                this.playerOnegaiRepository.Store(playerOnegaiModel);
            }
        }
    }    
}