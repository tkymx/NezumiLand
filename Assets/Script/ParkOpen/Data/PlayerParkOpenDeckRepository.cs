using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

namespace NL {

    [System.Serializable]
    public class PlayerParkOpenDeckEntry : EntryBase 
    {        
        public bool HasPlayerParkOpenCardId1;
        public uint PlayerParkOpenCardId1;
        public bool HasPlayerParkOpenCardId2;
        public uint PlayerParkOpenCardId2;
        public bool HasPlayerParkOpenCardId3;
        public uint PlayerParkOpenCardId3;
    }

    public interface IPlayerParkOpenDeckRepository 
    {
        PlayerParkOpenDeckModel Get (uint id);        
        List<PlayerParkOpenDeckModel> GetAll ();      
        PlayerParkOpenDeckModel Create ();
        void Store (PlayerParkOpenDeckModel playerParkOpenDeckModel);
        void Remove (PlayerParkOpenDeckModel playerParkOpenDeckModel);
    }

    public class PlayerParkOpenDeckRepository : PlayerRepositoryBase<PlayerParkOpenDeckEntry>, IPlayerParkOpenDeckRepository {

        private readonly IPlayerParkOpenCardRepository playerParkOpenCardRepository;

        public PlayerParkOpenDeckRepository (IPlayerParkOpenCardRepository playerParkOpenCardRepository, PlayerContextMap playerContextMap) : base (playerContextMap.PlayerParkOpenDeckEntrys) {
            this.playerParkOpenCardRepository = playerParkOpenCardRepository;
        }

        private PlayerParkOpenDeckModel CreateByEntry (PlayerParkOpenDeckEntry entry) {

            PlayerParkOpenCardModel playerParkOpenCardModel1 = null;
            if (entry.HasPlayerParkOpenCardId1) {
                playerParkOpenCardModel1 = this.playerParkOpenCardRepository.Get(entry.PlayerParkOpenCardId1);
                Debug.Assert(playerParkOpenCardModel1!=null, "playerParkOpenCardModel1 が存在していません");
            }

            PlayerParkOpenCardModel playerParkOpenCardModel2 = null;
            if (entry.HasPlayerParkOpenCardId2) {
                playerParkOpenCardModel2 = this.playerParkOpenCardRepository.Get(entry.PlayerParkOpenCardId2);
                Debug.Assert(playerParkOpenCardModel2!=null, "playerParkOpenCardModel2 が存在していません");
            }

            PlayerParkOpenCardModel playerParkOpenCardModel3 = null;
            if (entry.HasPlayerParkOpenCardId3) {
                playerParkOpenCardModel3 = this.playerParkOpenCardRepository.Get(entry.PlayerParkOpenCardId3);
                Debug.Assert(playerParkOpenCardModel3!=null, "playerParkOpenCardModel3 が存在していません");
            }

            return new PlayerParkOpenDeckModel(
                entry.Id,
                playerParkOpenCardModel1,
                playerParkOpenCardModel2,
                playerParkOpenCardModel3
            );
        }

        public PlayerParkOpenDeckModel Get (uint id) {
            var foundEntry = this.GetEntry(id);
            if (foundEntry == null) {
                return null;
            }
            return CreateByEntry(foundEntry);
        }

        public List<PlayerParkOpenDeckModel> GetAll () {
            return this.entrys.Select(entry => CreateByEntry(entry)).ToList();
        }

        public PlayerParkOpenDeckModel Create () 
        {
            var id = this.MaximuId()+1;

            var entry = new PlayerParkOpenDeckEntry () {
                Id = id,
                HasPlayerParkOpenCardId1 = false,
                HasPlayerParkOpenCardId2 = false,
                HasPlayerParkOpenCardId3 = false,
                PlayerParkOpenCardId1 = 0,
                PlayerParkOpenCardId2 = 0,
                PlayerParkOpenCardId3 = 0
            };
            this.entrys.Add(entry);
            PlayerContextMap.WriteEntry (this.entrys);
            return CreateByEntry(entry);
        }

        public void Store (PlayerParkOpenDeckModel playerParkOpenDeckModel) {
            var entry = this.GetEntry(playerParkOpenDeckModel.Id);
            if (entry != null) {
                var index = this.entrys.IndexOf (entry);
                this.entrys[index] = new PlayerParkOpenDeckEntry () {
                    Id = playerParkOpenDeckModel.Id,
                    HasPlayerParkOpenCardId1 = playerParkOpenDeckModel.PlayerParkOpenCardModel1 != null,
                    HasPlayerParkOpenCardId2 = playerParkOpenDeckModel.PlayerParkOpenCardModel2 != null,
                    HasPlayerParkOpenCardId3 = playerParkOpenDeckModel.PlayerParkOpenCardModel3 != null,
                    PlayerParkOpenCardId1 = playerParkOpenDeckModel.PlayerParkOpenCardModel1 != null ? playerParkOpenDeckModel.PlayerParkOpenCardModel1.Id : 0,
                    PlayerParkOpenCardId2 = playerParkOpenDeckModel.PlayerParkOpenCardModel2 != null ? playerParkOpenDeckModel.PlayerParkOpenCardModel2.Id : 0,
                    PlayerParkOpenCardId3 = playerParkOpenDeckModel.PlayerParkOpenCardModel3 != null ? playerParkOpenDeckModel.PlayerParkOpenCardModel3.Id : 0
                };
            } else {
                Debug.Assert(false,"要素が存在しません : " + playerParkOpenDeckModel.Id.ToString());
            }
            PlayerContextMap.WriteEntry (this.entrys);            
        }

        public void Remove (PlayerParkOpenDeckModel playerParkOpenDeckModel) {
            var entry = this.GetEntry(playerParkOpenDeckModel.Id);
            if (entry != null) {
                var index = this.entrys.IndexOf (entry);
                this.entrys.RemoveAt(index);
            } else {
                Debug.Assert(false,"要素が存在しません : " + playerParkOpenDeckModel.Id.ToString());
            }
            PlayerContextMap.WriteEntry (this.entrys);            
        }
    }
}
