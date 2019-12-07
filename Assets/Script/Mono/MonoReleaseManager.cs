using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  NL
{
    public class MonoReleaseManager
    {
        private readonly IPlayerMonoInfoRepository playerMonoInfoRepository = null;
        private Queue<uint> releaseReserveMonoIdQueue = null;

        public  MonoReleaseManager (IPlayerMonoInfoRepository playerMonoInfoRepository) {
            this.playerMonoInfoRepository = playerMonoInfoRepository;
            this.releaseReserveMonoIdQueue = new Queue<uint>();
        }

        public void EnqueueReserveReleaseMonoId(uint id) {
            this.releaseReserveMonoIdQueue.Enqueue(id);
        }

        public void UpdateByFrame() {
            while(this.releaseReserveMonoIdQueue.Count > 0) {
                var monoId = releaseReserveMonoIdQueue.Dequeue();
                var playerMonoInfo = playerMonoInfoRepository.GetById(monoId);

                // リリース済みにする
                playerMonoInfo.ToRelease();

                playerMonoInfoRepository.Store(playerMonoInfo);
            }
        }
    }   
}
