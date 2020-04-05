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
        public uint PlayerParkOpenGroupId;
        public int CurrentHeartCount;
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

        private readonly IPlayerParkOpenGroupRepository playerParkOpenGroupRepository;

        public PlayerParkOpenRepository (IPlayerParkOpenGroupRepository playerParkOpenGroupRepository, PlayerContextMap playerContextMap) : base (playerContextMap.PlayerParkOpenEntrys) 
        {
            this.playerParkOpenGroupRepository = playerParkOpenGroupRepository;
        }

        public PlayerParkOpenModel GetOwn () {
            var foundEntry = this.GetEntry(this.ownId);
            if (foundEntry == null) {
                var playerParkOpenModel = new PlayerParkOpenModel(
                    this.ownId,
                    false,
                    0,
                    0,
                    null,
                    0
                );
                return playerParkOpenModel;
            }

            PlayerParkOpenGroupModel playerParkOpeGroupModel = null;
            if (foundEntry.IsOpen) {
                playerParkOpeGroupModel = this.playerParkOpenGroupRepository.Get(foundEntry.PlayerParkOpenGroupId);
                Debug.Assert(playerParkOpeGroupModel != null, "parkOpeGroupModelがありません" + foundEntry.PlayerParkOpenGroupId.ToString());
            }

            return new PlayerParkOpenModel (
                foundEntry.Id, 
                foundEntry.IsOpen, 
                foundEntry.ElapsedTime, 
                foundEntry.NextWave, 
                playerParkOpeGroupModel,
                foundEntry.CurrentHeartCount);
        }

        public void Store (PlayerParkOpenModel playerParkOpenModel) {

            var toEntry = new PlayerParkOpenEntry () {
                Id = playerParkOpenModel.Id,
                IsOpen = playerParkOpenModel.IsOpen,
                ElapsedTime = playerParkOpenModel.ElapsedTime,
                NextWave = playerParkOpenModel.NextWave,
                PlayerParkOpenGroupId = playerParkOpenModel.PlayerParkOpenGroupModel != null ? playerParkOpenModel.PlayerParkOpenGroupModel.Id : 0,
                CurrentHeartCount = playerParkOpenModel.currentHeartCount
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