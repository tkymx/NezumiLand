using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

namespace NL {
    [System.Serializable]
    public class PlayerMouseStockEntry : EntryBase {

        
        public long MouseStockCount;
    }

    public interface IPlayerMouseStockRepository {
        PlayerMouseStockModel GetOwn ();
        void Store (PlayerMouseStockModel playerMouseStockModel);
    }

    /// <summary>
    /// マウスのストック情報に関する
    /// </summary>
    public class PlayerMouseStockRepository : PlayerRepositoryBase<PlayerMouseStockEntry>, IPlayerMouseStockRepository {
        private readonly uint ownId = 0;
        private readonly int defaultMouseStockCount = 2;

        public PlayerMouseStockRepository (PlayerContextMap playerContextMap) : base (playerContextMap.PlayerMouseStockEntrys) {
        }

        public PlayerMouseStockModel GetOwn () {
            var foundEntry = this.GetEntry(this.ownId);
            if (foundEntry == null) {
                var playerMouseStockModel = new PlayerMouseStockModel(
                    this.ownId,
                    this.defaultMouseStockCount
                );
                return playerMouseStockModel;
            }
            return new PlayerMouseStockModel (foundEntry.Id, foundEntry.MouseStockCount);
        }

        public void Store (PlayerMouseStockModel playerMouseStockModel) {
            var entry = this.GetEntry(playerMouseStockModel.Id);
            if (entry != null) {
                var index = this.entrys.IndexOf (entry);
                this.entrys[index] = new PlayerMouseStockEntry () {
                    Id = playerMouseStockModel.Id,
                    MouseStockCount = playerMouseStockModel.MouseStockCount.Value
                };
            } else {
                this.entrys.Add(new PlayerMouseStockEntry () {
                    Id = playerMouseStockModel.Id,
                    MouseStockCount = playerMouseStockModel.MouseStockCount.Value
                });
            }
            PlayerContextMap.WriteEntry (this.entrys);
        }
    }
}