using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  NL
{
    public class OnegaiManager
    {
        private readonly IPlayerOnegaiRepository playerOnegaiRepository;

        private Queue<uint> reserveUnLockOnegaiIdQueue = null;
        private Queue<uint> reserveLockOnegaiIdQueue = null;

        public OnegaiManager (IPlayerOnegaiRepository playerOnegaiRepository) {
            this.playerOnegaiRepository = playerOnegaiRepository;
            this.reserveUnLockOnegaiIdQueue = new Queue<uint>();
            this.reserveLockOnegaiIdQueue = new Queue<uint>();
        }

        public void EnqueueUnLockReserve(uint onegaiId) {
            this.reserveUnLockOnegaiIdQueue.Enqueue(onegaiId);
        }

        public void EnqueueLockReserve(uint onegaiId) {
            this.reserveLockOnegaiIdQueue.Enqueue(onegaiId);
        }

        public void UpdateImmidiately () {
            
            Debug.Assert(this.reserveUnLockOnegaiIdQueue != null, "reserveUnLockQueue が null です");
            Debug.Assert(this.reserveLockOnegaiIdQueue != null, "reserveLockQueue が null です");

            // UnLock 予約を UnLock にする。
            while(this.reserveUnLockOnegaiIdQueue.Count > 0) {

                // プレイヤーデータを取得
                var onegaiId = this.reserveUnLockOnegaiIdQueue.Dequeue();
                var playerOnegaiModel = this.playerOnegaiRepository.GetById(onegaiId);
                Debug.Assert(playerOnegaiModel != null, "playerOnegaiModel が null です");
                
                // アンロックにする
                Debug.Assert(playerOnegaiModel.IsLock(), playerOnegaiModel.Id.ToString() + "がLock状態ではありません");
                playerOnegaiModel.ToUnlock();
                playerOnegaiModel.UpdateStartTime();

                // キャッシュに入れる
                GameManager.Instance.OnegaiMediaterManager.ChacheOnegai(playerOnegaiModel.OnegaiModel);
                
                // 保存する
                this.playerOnegaiRepository.Store(playerOnegaiModel);
            }

            // Lock にする
            while(this.reserveLockOnegaiIdQueue.Count > 0) {

                // プレイヤーデータを取得
                var onegaiId = this.reserveLockOnegaiIdQueue.Dequeue();
                var playerOnegaiModel = this.playerOnegaiRepository.GetById(onegaiId);
                Debug.Assert(playerOnegaiModel != null, "playerOnegaiModel が null です");
                
                // アンロックにする
                Debug.Assert(!playerOnegaiModel.IsLock(), playerOnegaiModel.Id.ToString() + "がLock状態ではありません");
                playerOnegaiModel.ToLock();

                // キャッシュに入れる
                GameManager.Instance.OnegaiMediaterManager.UnChacheOnegai(playerOnegaiModel.OnegaiModel);
                
                // 保存する
                this.playerOnegaiRepository.Store(playerOnegaiModel);
            }            
        }

        public void UpdateByFrame() {

            //期限付きのものは期限があるかを確認
            foreach (var playerOnegaiModel in playerOnegaiRepository.GetAlreadyClose() )
            {
                this.reserveLockOnegaiIdQueue.Enqueue(playerOnegaiModel.Id);                
            }

            this.UpdateImmidiately();
        }
    }    
}