using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

namespace NL {
    [DataContract]
    public class PlayerInfoEntry : EntryBase {

        [DataMember]
        public float ElapsedTime { get; set; }

        [DataMember]
        public long Currency { get; set; }

        [DataMember]
        public long ArrangementItemAmount { get; set; }
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

        public PlayerInfoRepository (PlayerContextMap playerContextMap) : base (playerContextMap.PlayerInfoEntrys) {
        }

        public PlayerInfoModel GetOwn () {
            var foundEntry = this.GetEntry(this.ownId);
            if (foundEntry == null) {
                var playerInfoModel = new PlayerInfoModel(
                    this.ownId,
                    0,
                    new Currency(this.defaultCurrency),
                    new ArrangementItemAmount(this.defaultArrangementItemAmount)
                );
                return playerInfoModel;
            }
            return new PlayerInfoModel (
                foundEntry.Id, 
                foundEntry.ElapsedTime,
                new Currency(foundEntry.Currency),
                new ArrangementItemAmount(foundEntry.ArrangementItemAmount));
        }

        public void Store (PlayerInfoModel playerInfoModel) {
            var entry = this.GetEntry(playerInfoModel.Id);
            var newEntry = new PlayerInfoEntry () {
                    Id = playerInfoModel.Id,
                    ElapsedTime = playerInfoModel.ElapsedTime,
                    Currency = playerInfoModel.Currency.Value,
                    ArrangementItemAmount = playerInfoModel.ArrangementItemAmount.Value
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