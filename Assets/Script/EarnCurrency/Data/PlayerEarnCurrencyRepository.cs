using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;
using System;

namespace NL {
    [System.Serializable]
    public class PlayerEarnCurrencyEntry : EntryBase {
                
        public long EarnCurrency;
        public uint PlayerArrangementTargetId;
    }

    public interface IPlayerEarnCurrencyRepository {
        PlayerEarnCurrencyModel Get (uint id);
        List<PlayerEarnCurrencyModel> GetAll ();
        PlayerEarnCurrencyModel Create (PlayerArrangementTargetModel playerArrangementTargetModel, Currency earnCurrency);
        void Store (PlayerEarnCurrencyModel playerEarnCurrencyModel);
        void Remove (PlayerEarnCurrencyModel playerEarnCurrencyModel);
    }

    /// <summary>
    /// マウスのストック情報に関する
    /// </summary>
    public class PlayerEarnCurrencyRepository : PlayerRepositoryBase<PlayerEarnCurrencyEntry>, IPlayerEarnCurrencyRepository {

        private readonly IPlayerArrangementTargetRepository playerArrangementTargetRepository = null;

        public PlayerEarnCurrencyRepository (IPlayerArrangementTargetRepository playerArrangementTargetRepository, PlayerContextMap playerContextMap) : base (playerContextMap.PlayerEarnCurrencyEntrys) {
            this.playerArrangementTargetRepository = playerArrangementTargetRepository;
        }

        private PlayerEarnCurrencyModel CreateByEntry (PlayerEarnCurrencyEntry entry) {

            var playerArrangementTargetModel = this.playerArrangementTargetRepository.Get(entry.PlayerArrangementTargetId);
            Debug.Assert(playerArrangementTargetModel != null, "PlayerArrangementTargetModelが存在しません。:" + playerArrangementTargetModel.Id.ToString()) ;

            return new PlayerEarnCurrencyModel(
                entry.Id,
                new Currency(entry.EarnCurrency),
                playerArrangementTargetModel
            );
        }

        public PlayerEarnCurrencyModel Get (uint id) {
            var foundEntry = this.GetEntry(id);
            if (foundEntry == null) {
                return null;
            }
            return CreateByEntry(foundEntry);
        }

        public List<PlayerEarnCurrencyModel> GetAll () {
            return this.entrys.Select(entry => CreateByEntry(entry)).ToList();
        }

        public PlayerEarnCurrencyModel Create (
            PlayerArrangementTargetModel playerArrangementTargetModel,
            Currency earnCurrency
        ) {
            var id = this.MaximuId()+1;
            var entry = new PlayerEarnCurrencyEntry () {
                Id = id,
                EarnCurrency = earnCurrency.Value,
                PlayerArrangementTargetId = playerArrangementTargetModel.Id
            };
            this.entrys.Add(entry);
            PlayerContextMap.WriteEntry (this.entrys);
            return CreateByEntry(entry);
        }

        public void Store (PlayerEarnCurrencyModel playerEarnCurrencyModel) {
            var entry = this.GetEntry(playerEarnCurrencyModel.Id);
            if (entry != null) {
                var index = this.entrys.IndexOf (entry);
                this.entrys[index] = new PlayerEarnCurrencyEntry () {
                    Id = playerEarnCurrencyModel.Id,
                    EarnCurrency = playerEarnCurrencyModel.EarnCurrency.Value,
                    PlayerArrangementTargetId = playerEarnCurrencyModel.PlayerArrangementTargetModel.Id
                };
            } else {
                Debug.Assert(false,"要素が存在しません : " + playerEarnCurrencyModel.Id.ToString());
            }
            PlayerContextMap.WriteEntry (this.entrys);            
        }

        public void Remove (PlayerEarnCurrencyModel playerEarnCurrencyModel) {
            var entry = this.GetEntry(playerEarnCurrencyModel.Id);
            if (entry != null) {
                var index = this.entrys.IndexOf (entry);
                this.entrys.RemoveAt(index);
            } else {
                Debug.Assert(false,"要素が存在しません : " + playerEarnCurrencyModel.Id.ToString());
            }
            PlayerContextMap.WriteEntry (this.entrys);            
        }
    }
}