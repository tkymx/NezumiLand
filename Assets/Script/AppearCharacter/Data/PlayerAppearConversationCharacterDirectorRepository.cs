using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;
using System;

namespace NL {

    [System.Serializable]
    public class PlayerAppearConversationCharacterDirectorEntry : EntryBase 
    {        
        public uint AppearConversationCharacterDirectorId;
        public uint PlayerAppearCharacterReserveId;
        public bool IsReceiveReward;
    }

    public interface IPlayerAppearConversationCharacterDirectorRepository 
    {
        PlayerAppearConversationCharacterDirectorModel Get (uint id);        
        List<PlayerAppearConversationCharacterDirectorModel> GetAll ();      
        PlayerAppearConversationCharacterDirectorModel Create (  
            AppearConversationCharacterDirectorModel appearConversationCharacterDirectorModel,         
            PlayerAppearCharacterReserveModel playerAppearCharacterReserveModel);
        void Store (PlayerAppearConversationCharacterDirectorModel PlayerAppearConversationCharacterDirectorModel);
        void Remove (PlayerAppearConversationCharacterDirectorModel PlayerAppearConversationCharacterDirectorModel);
    }

    public class PlayerAppearConversationCharacterDirectorRepository : PlayerRepositoryBase<PlayerAppearConversationCharacterDirectorEntry>, IPlayerAppearConversationCharacterDirectorRepository {

        private readonly AppearConversationCharacterDirectorRepository appearConversationCharacterDirectorRepository;
        private readonly PlayerAppearCharacterReserveRepository playerAppearCharacterReserveRepository;

        public PlayerAppearConversationCharacterDirectorRepository (AppearConversationCharacterDirectorRepository appearConversationCharacterDirectorRepository, PlayerAppearCharacterReserveRepository playerAppearCharacterReserveRepository, PlayerContextMap playerContextMap) : base (playerContextMap.PlayerAppearConversationCharacterDirectorEntrys) {
            this.appearConversationCharacterDirectorRepository = appearConversationCharacterDirectorRepository;
            this.playerAppearCharacterReserveRepository = playerAppearCharacterReserveRepository;
        }

        private PlayerAppearConversationCharacterDirectorModel CreateByEntry (PlayerAppearConversationCharacterDirectorEntry entry) {

            var appearConversationCharacterDirectorModel = this.appearConversationCharacterDirectorRepository.Get(entry.AppearConversationCharacterDirectorId);      
            Debug.Assert(appearConversationCharacterDirectorModel != null, "appearConversationCharacterDirectorModel が存在しません。");
                  
            var playerAppearCharacterReserveModel = this.playerAppearCharacterReserveRepository.Get(entry.PlayerAppearCharacterReserveId);            
            Debug.Assert(playerAppearCharacterReserveModel != null, "playerAppearCharacterReserveModel が存在しません。");
            
            return new PlayerAppearConversationCharacterDirectorModel(
                entry.Id,
                appearConversationCharacterDirectorModel,
                playerAppearCharacterReserveModel,
                new AppearCharactorWithReward(entry.IsReceiveReward)
            );
        }

        public PlayerAppearConversationCharacterDirectorModel Get (uint id) {
            var foundEntry = this.GetEntry(id);
            if (foundEntry == null) {
                return null;
            }
            return CreateByEntry(foundEntry);
        }

        public List<PlayerAppearConversationCharacterDirectorModel> GetAll () {
            return this.entrys.Select(entry => CreateByEntry(entry)).ToList();
        }

        public PlayerAppearConversationCharacterDirectorModel Create (
            AppearConversationCharacterDirectorModel appearConversationCharacterDirectorModel,         
            PlayerAppearCharacterReserveModel playerAppearCharacterReserveModel)
        {
            var id = this.MaximuId()+1;

            var entry = new PlayerAppearConversationCharacterDirectorEntry () {
                Id = id,
                AppearConversationCharacterDirectorId = appearConversationCharacterDirectorModel.Id,
                PlayerAppearCharacterReserveId = playerAppearCharacterReserveModel.Id,
                IsReceiveReward = false /*はじめはまだ受け取っていない*/,
            };
            this.entrys.Add(entry);
            PlayerContextMap.WriteEntry (this.entrys);
            return CreateByEntry(entry);
        }

        public void Store (PlayerAppearConversationCharacterDirectorModel PlayerAppearConversationCharacterDirectorModel) {
            var entry = this.GetEntry(PlayerAppearConversationCharacterDirectorModel.Id);
            if (entry != null) {
                var index = this.entrys.IndexOf (entry);
                this.entrys[index] = new PlayerAppearConversationCharacterDirectorEntry () {
                    Id = PlayerAppearConversationCharacterDirectorModel.Id,
                    AppearConversationCharacterDirectorId = PlayerAppearConversationCharacterDirectorModel.AppearConversationCharacterDirectorModel.Id,
                    PlayerAppearCharacterReserveId = PlayerAppearConversationCharacterDirectorModel.PlayerAppearCharacterReserveModel.Id,
                    IsReceiveReward = PlayerAppearConversationCharacterDirectorModel.AppearCharactorWithReward.IsReceiveReward
                };
            } else {
                Debug.Assert(false,"要素が存在しません : " + PlayerAppearConversationCharacterDirectorModel.Id.ToString());
            }
            PlayerContextMap.WriteEntry (this.entrys);            
        }

        public void Remove (PlayerAppearConversationCharacterDirectorModel PlayerAppearConversationCharacterDirectorModel) {
            var entry = this.GetEntry(PlayerAppearConversationCharacterDirectorModel.Id);
            if (entry != null) {
                var index = this.entrys.IndexOf (entry);
                this.entrys.RemoveAt(index);
            } else {
                Debug.Assert(false,"要素が存在しません : " + PlayerAppearConversationCharacterDirectorModel.Id.ToString());
            }
            PlayerContextMap.WriteEntry (this.entrys);            
        }
    }
}
