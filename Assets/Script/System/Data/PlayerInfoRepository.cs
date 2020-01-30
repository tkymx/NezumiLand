using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

namespace NL {
    [System.Serializable]
    public class PlayerInfoEntry : EntryBase {
        public float ElapsedTime;
        public long Currency;        
        public long ArrangementItemAmount;
        public bool HasPlayerParkOpenDeckId;
        public uint PlayerParkOpenDeckId;
    }

    public interface IPlayerInfoRepository {
        PlayerInfoModel GetOwn ();
        void Store (PlayerInfoModel playerInfoModel);
    }

    /// <summary>
    /// マウスのストック情報に関する
    /// </summary>
    public class PlayerInfoRepository : PlayerRepositoryBase<PlayerInfoEntry>, IPlayerInfoRepository {
        private readonly uint ownId = 0;
        private readonly long defaultCurrency = 100;
        private readonly long defaultArrangementItemAmount = 100;
        private readonly IPlayerParkOpenDeckRepository playerParkOpenDeckRepository = null;

        public PlayerInfoRepository (IPlayerParkOpenDeckRepository playerParkOpenDeckRepository, PlayerContextMap playerContextMap) : base (playerContextMap.PlayerInfoEntrys) {
            this.playerParkOpenDeckRepository = playerParkOpenDeckRepository;
        }

        public PlayerInfoModel GetOwn () {
            var foundEntry = this.GetEntry(this.ownId);
            if (foundEntry == null) {
                var playerInfoModel = new PlayerInfoModel(
                    this.ownId,
                    0,
                    new Currency(this.defaultCurrency),
                    new ArrangementItemAmount(this.defaultArrangementItemAmount),
                    null
                );
                return playerInfoModel;
            }

            PlayerParkOpenDeckModel playerParkOpenDeckModel = null;
            if (foundEntry.HasPlayerParkOpenDeckId) {
                playerParkOpenDeckModel = this.playerParkOpenDeckRepository.Get(foundEntry.PlayerParkOpenDeckId);
                Debug.Assert(playerParkOpenDeckModel != null, "playerParkOpenDeckModelがありません");
            }

            return new PlayerInfoModel (
                foundEntry.Id, 
                foundEntry.ElapsedTime,
                new Currency(foundEntry.Currency),
                new ArrangementItemAmount(foundEntry.ArrangementItemAmount),
                playerParkOpenDeckModel);
        }

        public void Store (PlayerInfoModel playerInfoModel) {
            var entry = this.GetEntry(playerInfoModel.Id);
            var newEntry = new PlayerInfoEntry () {
                    Id = playerInfoModel.Id,
                    ElapsedTime = playerInfoModel.ElapsedTime,
                    Currency = playerInfoModel.Currency.Value,
                    ArrangementItemAmount = playerInfoModel.ArrangementItemAmount.Value,
                    HasPlayerParkOpenDeckId = playerInfoModel.CurrentParkOpenDeckModel != null,
                    PlayerParkOpenDeckId = playerInfoModel.CurrentParkOpenDeckModel != null ? playerInfoModel.CurrentParkOpenDeckModel.Id : 0
                };
            if (entry != null) {
                var index = this.entrys.IndexOf (entry);
                this.entrys[index] = newEntry;
            } else {
                this.entrys.Add(newEntry);
            }
            PlayerContextMap.WriteEntry (this.entrys);
        }
    }
}