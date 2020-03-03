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
        public bool HasPlayerParkOpenDeckId;
        public uint PlayerParkOpenDeckId;
        public bool CanUseCard1;
        public bool CanUseCard2;
        public bool CanUseCard3;
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
        private readonly IPlayerParkOpenDeckRepository playerParkOpenDeckRepository = null;

        public PlayerParkOpenRepository (IPlayerParkOpenGroupRepository playerParkOpenGroupRepository, IPlayerParkOpenDeckRepository playerParkOpenDeckRepository, PlayerContextMap playerContextMap) : base (playerContextMap.PlayerParkOpenEntrys) 
        {
            this.playerParkOpenGroupRepository = playerParkOpenGroupRepository;
            this.playerParkOpenDeckRepository = playerParkOpenDeckRepository;
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
                    0,
                    null,
                    false,
                    false,
                    false
                );
                return playerParkOpenModel;
            }

            PlayerParkOpenGroupModel playerParkOpeGroupModel = null;
            if (foundEntry.IsOpen) {
                playerParkOpeGroupModel = this.playerParkOpenGroupRepository.Get(foundEntry.PlayerParkOpenGroupId);
                Debug.Assert(playerParkOpeGroupModel != null, "parkOpeGroupModelがありません" + foundEntry.PlayerParkOpenGroupId.ToString());
            }

            PlayerParkOpenDeckModel playerParkOpenDeckModel = null;
            if (foundEntry.HasPlayerParkOpenDeckId) {
                playerParkOpenDeckModel = this.playerParkOpenDeckRepository.Get(foundEntry.PlayerParkOpenDeckId);
                Debug.Assert(playerParkOpenDeckModel != null, "playerParkOpenDeckModelがありません");
            }

            return new PlayerParkOpenModel (
                foundEntry.Id, 
                foundEntry.IsOpen, 
                foundEntry.ElapsedTime, 
                foundEntry.NextWave, 
                playerParkOpeGroupModel,
                foundEntry.CurrentHeartCount,
                playerParkOpenDeckModel,
                foundEntry.CanUseCard1,
                foundEntry.CanUseCard2,
                foundEntry.CanUseCard3);
        }

        public void Store (PlayerParkOpenModel playerParkOpenModel) {

            var toEntry = new PlayerParkOpenEntry () {
                Id = playerParkOpenModel.Id,
                IsOpen = playerParkOpenModel.IsOpen,
                ElapsedTime = playerParkOpenModel.ElapsedTime,
                NextWave = playerParkOpenModel.NextWave,
                PlayerParkOpenGroupId = playerParkOpenModel.PlayerParkOpenGroupModel != null ? playerParkOpenModel.PlayerParkOpenGroupModel.Id : 0,
                CurrentHeartCount = playerParkOpenModel.currentHeartCount,
                HasPlayerParkOpenDeckId = playerParkOpenModel.CurrentParkOpenDeckModel != null,
                PlayerParkOpenDeckId = playerParkOpenModel.CurrentParkOpenDeckModel != null ? playerParkOpenModel.CurrentParkOpenDeckModel.Id : 0,
                CanUseCard1 = playerParkOpenModel.CanUseCard1,
                CanUseCard2 = playerParkOpenModel.CanUseCard2,
                CanUseCard3 = playerParkOpenModel.CanUseCard3
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