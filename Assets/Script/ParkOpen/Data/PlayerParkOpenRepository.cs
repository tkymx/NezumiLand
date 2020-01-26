using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

namespace NL {
    [System.Serializable]
    public class PlayerParkOpenEntry : EntryBase {
        public bool IsOpen;
        public float ElapsedTime;
        public int NextWave;
        public uint ParkOpenGroupId;

    }

    public interface IPlayerParkOpenRepository {
        PlayerParkOpenModel GetOwn ();
        void Store (PlayerParkOpenModel playerParkOpenModel);
    }

    /// <summary>
    /// マウスのストック情報に関する
    /// </summary>
    public class PlayerParkOpenRepository : PlayerRepositoryBase<PlayerParkOpenEntry>, IPlayerParkOpenRepository {
        private readonly uint ownId = 0;

        private readonly IParkOpenGroupRepository parkOpenGroupRepository;

        public PlayerParkOpenRepository (IParkOpenGroupRepository parkOpenGroupRepository, PlayerContextMap playerContextMap) : base (playerContextMap.PlayerParkOpenEntrys) 
        {
            this.parkOpenGroupRepository = parkOpenGroupRepository;
        }

        public PlayerParkOpenModel GetOwn () {
            var foundEntry = this.GetEntry(this.ownId);
            if (foundEntry == null) {
                var playerParkOpenModel = new PlayerParkOpenModel(
                    this.ownId,
                    false,
                    0,
                    0,
                    null
                );
                return playerParkOpenModel;
            }

            ParkOpenGroupModel parkOpeGroupModel = null;
            if (foundEntry.IsOpen) {
                parkOpeGroupModel = this.parkOpenGroupRepository.Get(foundEntry.ParkOpenGroupId);
                Debug.Assert(parkOpeGroupModel != null, "parkOpeGroupModelがありません" + foundEntry.ParkOpenGroupId.ToString());
            }

            return new PlayerParkOpenModel (
                foundEntry.Id, 
                foundEntry.IsOpen, 
                foundEntry.ElapsedTime, 
                foundEntry.NextWave, 
                parkOpeGroupModel);
        }

        public void Store (PlayerParkOpenModel playerParkOpenModel) {

            var toEntry = new PlayerParkOpenEntry () {
                Id = playerParkOpenModel.Id,
                IsOpen = playerParkOpenModel.IsOpen,
                ElapsedTime = playerParkOpenModel.ElapsedTime,
                NextWave = playerParkOpenModel.NextWave,
                ParkOpenGroupId = playerParkOpenModel.ParkOpenGroupModel != null ? playerParkOpenModel.ParkOpenGroupModel.Id : 0
            };

            var entry = this.GetEntry(playerParkOpenModel.Id);
            if (entry != null) {
                var index = this.entrys.IndexOf (entry);
                this.entrys[index] = toEntry;
            } else {
                this.entrys.Add(toEntry);
            }
            PlayerContextMap.WriteEntry (this.entrys);
        }
    }
}