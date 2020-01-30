using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

namespace NL {

    [System.Serializable]
    public class PlayerParkOpenCardEntry : EntryBase 
    {        
        public uint ParkOpenCardId;
    }

    public interface IPlayerParkOpenCardRepository 
    {
        PlayerParkOpenCardModel Get (uint id);        
        List<PlayerParkOpenCardModel> GetAll ();      
        PlayerParkOpenCardModel Create (ParkOpenCardModel parkOpenCardModel);
        void Store (PlayerParkOpenCardModel playerParkOpenCardModel);
        void Remove (PlayerParkOpenCardModel playerParkOpenCardModel);
    }

    public class PlayerParkOpenCardRepository : PlayerRepositoryBase<PlayerParkOpenCardEntry>, IPlayerParkOpenCardRepository {

        private readonly IParkOpenCardRepository parkOpenCardRepository;

        public PlayerParkOpenCardRepository (IParkOpenCardRepository parkOpenCardRepository, PlayerContextMap playerContextMap) : base (playerContextMap.PlayerParkOpenCardEntrys) {
            this.parkOpenCardRepository = parkOpenCardRepository;
        }

        private PlayerParkOpenCardModel CreateByEntry (PlayerParkOpenCardEntry entry) {

            var parkOpenCardModel = this.parkOpenCardRepository.Get(entry.ParkOpenCardId);
            Debug.Assert(parkOpenCardModel!=null, "parkOpenCardMdeol が存在していません");

            return new PlayerParkOpenCardModel(
                entry.Id,
                parkOpenCardModel
            );
        }

        public PlayerParkOpenCardModel Get (uint id) {
            var foundEntry = this.GetEntry(id);
            if (foundEntry == null) {
                return null;
            }
            return CreateByEntry(foundEntry);
        }

        public List<PlayerParkOpenCardModel> GetAll () {
            return this.entrys.Select(entry => CreateByEntry(entry)).ToList();
        }

        public PlayerParkOpenCardModel Create (ParkOpenCardModel parkOpenCardModel) 
        {
            var id = this.MaximuId()+1;

            var entry = new PlayerParkOpenCardEntry () {
                Id = id,
                ParkOpenCardId = parkOpenCardModel.Id
            };
            this.entrys.Add(entry);
            PlayerContextMap.WriteEntry (this.entrys);
            return CreateByEntry(entry);
        }

        public void Store (PlayerParkOpenCardModel playerParkOpenCardModel) {
            var entry = this.GetEntry(playerParkOpenCardModel.Id);
            if (entry != null) {
                var index = this.entrys.IndexOf (entry);
                this.entrys[index] = new PlayerParkOpenCardEntry () {
                    Id = playerParkOpenCardModel.Id,
                    ParkOpenCardId = playerParkOpenCardModel.ParkOpenCardModel.Id
                };
            } else {
                Debug.Assert(false,"要素が存在しません : " + playerParkOpenCardModel.Id.ToString());
            }
            PlayerContextMap.WriteEntry (this.entrys);            
        }

        public void Remove (PlayerParkOpenCardModel playerParkOpenCardModel) {
            var entry = this.GetEntry(playerParkOpenCardModel.Id);
            if (entry != null) {
                var index = this.entrys.IndexOf (entry);
                this.entrys.RemoveAt(index);
            } else {
                Debug.Assert(false,"要素が存在しません : " + playerParkOpenCardModel.Id.ToString());
            }
            PlayerContextMap.WriteEntry (this.entrys);            
        }
    }
}
