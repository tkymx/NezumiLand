using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  NL
{
    public class OnegaiManager
    {
        private struct NextStateAmount
        {
            public OnegaiState OnegaiState;
            public uint OnegaiId;
        }

        private readonly IPlayerOnegaiRepository playerOnegaiRepository;

        private Queue<NextStateAmount> reserveNextStateAmountQueue = null;

        public OnegaiManager (IPlayerOnegaiRepository playerOnegaiRepository) {
            this.playerOnegaiRepository = playerOnegaiRepository;
            this.reserveNextStateAmountQueue = new Queue<NextStateAmount>();
        }

        public void EnqueueUnLockReserve(uint onegaiId) {
            this.reserveNextStateAmountQueue.Enqueue(
                new NextStateAmount(){
                    OnegaiState = OnegaiState.UnLock,
                    OnegaiId = onegaiId
                });
        }

        public void EnqueueLockReserve(uint onegaiId) {
            this.reserveNextStateAmountQueue.Enqueue(
                new NextStateAmount(){
                    OnegaiState = OnegaiState.Lock,
                    OnegaiId = onegaiId
                });
        }

        public void UpdateImmidiately () {
            
            Debug.Assert(this.reserveNextStateAmountQueue != null, "reserveUnLockQueue が null です");

            while(this.reserveNextStateAmountQueue.Count > 0) {

                var nextStateAmount = this.reserveNextStateAmountQueue.Dequeue();
                var playerOnegaiModel = this.playerOnegaiRepository.GetById(nextStateAmount.OnegaiId);
                Debug.Assert(playerOnegaiModel != null, "playerOnegaiModel が null です");

                if (nextStateAmount.OnegaiState == OnegaiState.UnLock)
                {
                    // アンロックにする
                    playerOnegaiModel.ToUnlock();
                    playerOnegaiModel.UpdateStartTime();

                    // キャッシュに入れる
                    GameManager.Instance.OnegaiMediaterManager.ChacheOnegai(playerOnegaiModel.OnegaiModel);
                }
                else if (nextStateAmount.OnegaiState == OnegaiState.Lock)
                {
                    // ロックにする
                    playerOnegaiModel.ToLock();

                    // キャッシュから外す
                    GameManager.Instance.OnegaiMediaterManager.UnChacheOnegai(playerOnegaiModel.OnegaiModel);                    
                }

                // 保存する
                this.playerOnegaiRepository.Store(playerOnegaiModel);
            }         
        }

        public void UpdateByFrame() {

            //期限付きのものは期限があるかを確認
            foreach (var playerOnegaiModel in playerOnegaiRepository.GetAlreadyClose() )
            {
                this.EnqueueLockReserve(playerOnegaiModel.Id);
            }

            this.UpdateImmidiately();
        }
    }    
}