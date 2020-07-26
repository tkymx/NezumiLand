using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;
using System;

namespace NL {

    [System.Serializable]
    public class PlayerAppearPlayingCharacterDirectorEntry : EntryBase 
    {        
        public uint AppearPlayingCharacterDirectorId;
        public uint PlayerAppearCharacterReserveId;
        public bool IsReceiveReward;
    }

    public interface IPlayerAppearPlayingCharacterDirectorRepository 
    {
        PlayerAppearPlayingCharacterDirectorModel Get (uint id);        
        List<PlayerAppearPlayingCharacterDirectorModel> GetAll ();      
        PlayerAppearPlayingCharacterDirectorModel Create (  
            AppearPlayingCharacterDirectorModel appearPlayingCharacterDirectorModel,         
            PlayerAppearCharacterReserveModel playerAppearCharacterReserveModel);
        void Store (PlayerAppearPlayingCharacterDirectorModel PlayerAppearPlayingCharacterDirectorModel);
        void Remove (PlayerAppearPlayingCharacterDirectorModel PlayerAppearPlayingCharacterDirectorModel);
    }

    public class PlayerAppearPlayingCharacterDirectorRepository : PlayerRepositoryBase<PlayerAppearPlayingCharacterDirectorEntry>, IPlayerAppearPlayingCharacterDirectorRepository {

        private readonly AppearPlayingCharacterDirectorRepository appearPlayingCharacterDirectorRepository;
        private readonly PlayerAppearCharacterReserveRepository playerAppearCharacterReserveRepository;

        public PlayerAppearPlayingCharacterDirectorRepository (AppearPlayingCharacterDirectorRepository appearPlayingCharacterDirectorRepository, PlayerAppearCharacterReserveRepository playerAppearCharacterReserveRepository, PlayerContextMap playerContextMap) : base (playerContextMap.PlayerAppearPlayingCharacterDirectorEntrys) {
            this.appearPlayingCharacterDirectorRepository = appearPlayingCharacterDirectorRepository;
            this.playerAppearCharacterReserveRepository = playerAppearCharacterReserveRepository;
        }

        private PlayerAppearPlayingCharacterDirectorModel CreateByEntry (PlayerAppearPlayingCharacterDirectorEntry entry) {

            var appearPlayingCharacterDirectorModel = this.appearPlayingCharacterDirectorRepository.Get(entry.AppearPlayingCharacterDirectorId);      
            Debug.Assert(appearPlayingCharacterDirectorModel != null, "appearPlayingCharacterDirectorModel が存在しません。");
                  
            var playerAppearCharacterReserveModel = this.playerAppearCharacterReserveRepository.Get(entry.PlayerAppearCharacterReserveId);            
            Debug.Assert(playerAppearCharacterReserveModel != null, "playerAppearCharacterReserveModel が存在しません。");
            
            return new PlayerAppearPlayingCharacterDirectorModel(
                entry.Id,
                appearPlayingCharacterDirectorModel,
                playerAppearCharacterReserveModel
            );
        }

        public PlayerAppearPlayingCharacterDirectorModel Get (uint id) {
            var foundEntry = this.GetEntry(id);
            if (foundEntry == null) {
                return null;
            }
            return CreateByEntry(foundEntry);
        }

        public List<PlayerAppearPlayingCharacterDirectorModel> GetAll () {
            return this.entrys.Select(entry => CreateByEntry(entry)).ToList();
        }

        public PlayerAppearPlayingCharacterDirectorModel Create (
            AppearPlayingCharacterDirectorModel appearPlayingCharacterDirectorModel,         
            PlayerAppearCharacterReserveModel playerAppearCharacterReserveModel)
        {
            var id = this.MaximuId()+1;

            var entry = new PlayerAppearPlayingCharacterDirectorEntry () {
                Id = id,
                AppearPlayingCharacterDirectorId = appearPlayingCharacterDirectorModel.Id,
                PlayerAppearCharacterReserveId = playerAppearCharacterReserveModel.Id,
                IsReceiveReward = false /*はじめはまだ受け取っていない*/
            };
            this.entrys.Add(entry);
            PlayerContextMap.WriteEntry (this.entrys);
            return CreateByEntry(entry);
        }

        public void Store (PlayerAppearPlayingCharacterDirectorModel PlayerAppearPlayingCharacterDirectorModel) {
            var entry = this.GetEntry(PlayerAppearPlayingCharacterDirectorModel.Id);
            if (entry != null) {
                var index = this.entrys.IndexOf (entry);
                this.entrys[index] = new PlayerAppearPlayingCharacterDirectorEntry () {
                    Id = PlayerAppearPlayingCharacterDirectorModel.Id,
                    AppearPlayingCharacterDirectorId = PlayerAppearPlayingCharacterDirectorModel.AppearPlayingCharacterDirectorModel.Id,
                    PlayerAppearCharacterReserveId = PlayerAppearPlayingCharacterDirectorModel.PlayerAppearCharacterReserveModel.Id
                };
            } else {
                Debug.Assert(false,"要素が存在しません : " + PlayerAppearPlayingCharacterDirectorModel.Id.ToString());
            }
            PlayerContextMap.WriteEntry (this.entrys);            
        }

        public void Remove (PlayerAppearPlayingCharacterDirectorModel PlayerAppearPlayingCharacterDirectorModel) {
            var entry = this.GetEntry(PlayerAppearPlayingCharacterDirectorModel.Id);
            if (entry != null) {
                var index = this.entrys.IndexOf (entry);
                this.entrys.RemoveAt(index);
            } else {
                Debug.Assert(false,"要素が存在しません : " + PlayerAppearPlayingCharacterDirectorModel.Id.ToString());
            }
            PlayerContextMap.WriteEntry (this.entrys);            
        }
    }
}
