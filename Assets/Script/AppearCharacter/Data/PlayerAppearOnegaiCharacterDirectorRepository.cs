using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;
using System;

namespace NL {

    [System.Serializable]
    public class PlayerAppearOnegaiCharacterDirectorEntry : EntryBase 
    {        
        public uint AppearOnegaiCharacterDirectorId;
        public uint PlayerAppearCharacterReserveId;
        public bool IsReceiveReward;
        public bool IsCancel;
    }

    public interface IPlayerAppearOnegaiCharacterDirectorRepository 
    {
        PlayerAppearOnegaiCharacterDirectorModel Get (uint id);        
        List<PlayerAppearOnegaiCharacterDirectorModel> GetAll ();      
        PlayerAppearOnegaiCharacterDirectorModel Create (  
            AppearOnegaiCharacterDirectorModel appearOnegaiCharacterDirectorModel,         
            PlayerAppearCharacterReserveModel playerAppearCharacterReserveModel);
        void Store (PlayerAppearOnegaiCharacterDirectorModel PlayerAppearOnegaiCharacterDirectorModel);
        void Remove (PlayerAppearOnegaiCharacterDirectorModel PlayerAppearOnegaiCharacterDirectorModel);
    }

    public class PlayerAppearOnegaiCharacterDirectorRepository : PlayerRepositoryBase<PlayerAppearOnegaiCharacterDirectorEntry>, IPlayerAppearOnegaiCharacterDirectorRepository {

        private readonly AppearOnegaiCharacterDirectorRepository appearOnegaiCharacterDirectorRepository;
        private readonly PlayerAppearCharacterReserveRepository playerAppearCharacterReserveRepository;

        public PlayerAppearOnegaiCharacterDirectorRepository (AppearOnegaiCharacterDirectorRepository appearOnegaiCharacterDirectorRepository, PlayerAppearCharacterReserveRepository playerAppearCharacterReserveRepository, PlayerContextMap playerContextMap) : base (playerContextMap.PlayerAppearOnegaiCharacterDirectorEntrys) {
            this.appearOnegaiCharacterDirectorRepository = appearOnegaiCharacterDirectorRepository;
            this.playerAppearCharacterReserveRepository = playerAppearCharacterReserveRepository;
        }

        private PlayerAppearOnegaiCharacterDirectorModel CreateByEntry (PlayerAppearOnegaiCharacterDirectorEntry entry) {

            var appearOnegaiCharacterDirectorModel = this.appearOnegaiCharacterDirectorRepository.Get(entry.AppearOnegaiCharacterDirectorId);      
            Debug.Assert(appearOnegaiCharacterDirectorModel != null, "appearOnegaiCharacterDirectorModel が存在しません。");
                  
            var playerAppearCharacterReserveModel = this.playerAppearCharacterReserveRepository.Get(entry.PlayerAppearCharacterReserveId);            
            Debug.Assert(playerAppearCharacterReserveModel != null, "playerAppearCharacterReserveModel が存在しません。");
            
            return new PlayerAppearOnegaiCharacterDirectorModel(
                entry.Id,
                appearOnegaiCharacterDirectorModel,
                playerAppearCharacterReserveModel,
                new AppearCharactorWithReward(entry.IsReceiveReward),
                entry.IsCancel
            );
        }

        public PlayerAppearOnegaiCharacterDirectorModel Get (uint id) {
            var foundEntry = this.GetEntry(id);
            if (foundEntry == null) {
                return null;
            }
            return CreateByEntry(foundEntry);
        }

        public List<PlayerAppearOnegaiCharacterDirectorModel> GetAll () {
            return this.entrys.Select(entry => CreateByEntry(entry)).ToList();
        }

        public PlayerAppearOnegaiCharacterDirectorModel Create (
            AppearOnegaiCharacterDirectorModel appearOnegaiCharacterDirectorModel,         
            PlayerAppearCharacterReserveModel playerAppearCharacterReserveModel)
        {
            var id = this.MaximuId()+1;

            var entry = new PlayerAppearOnegaiCharacterDirectorEntry () {
                Id = id,
                AppearOnegaiCharacterDirectorId = appearOnegaiCharacterDirectorModel.Id,
                PlayerAppearCharacterReserveId = playerAppearCharacterReserveModel.Id,
                IsReceiveReward = false /*はじめはまだ受け取っていない*/,
                IsCancel = false /*初回はキャンセルではない*/
            };
            this.entrys.Add(entry);
            PlayerContextMap.WriteEntry (this.entrys);
            return CreateByEntry(entry);
        }

        public void Store (PlayerAppearOnegaiCharacterDirectorModel PlayerAppearOnegaiCharacterDirectorModel) {
            var entry = this.GetEntry(PlayerAppearOnegaiCharacterDirectorModel.Id);
            if (entry != null) {
                var index = this.entrys.IndexOf (entry);
                this.entrys[index] = new PlayerAppearOnegaiCharacterDirectorEntry () {
                    Id = PlayerAppearOnegaiCharacterDirectorModel.Id,
                    AppearOnegaiCharacterDirectorId = PlayerAppearOnegaiCharacterDirectorModel.AppearOnegaiCharacterDirectorModel.Id,
                    PlayerAppearCharacterReserveId = PlayerAppearOnegaiCharacterDirectorModel.PlayerAppearCharacterReserveModel.Id,
                    IsReceiveReward = PlayerAppearOnegaiCharacterDirectorModel.AppearCharactorWithReward.IsReceiveReward,
                    IsCancel = PlayerAppearOnegaiCharacterDirectorModel.IsCancel
                };
            } else {
                Debug.Assert(false,"要素が存在しません : " + PlayerAppearOnegaiCharacterDirectorModel.Id.ToString());
            }
            PlayerContextMap.WriteEntry (this.entrys);            
        }

        public void Remove (PlayerAppearOnegaiCharacterDirectorModel PlayerAppearOnegaiCharacterDirectorModel) {
            var entry = this.GetEntry(PlayerAppearOnegaiCharacterDirectorModel.Id);
            if (entry != null) {
                var index = this.entrys.IndexOf (entry);
                this.entrys.RemoveAt(index);
            } else {
                Debug.Assert(false,"要素が存在しません : " + PlayerAppearOnegaiCharacterDirectorModel.Id.ToString());
            }
            PlayerContextMap.WriteEntry (this.entrys);            
        }
    }
}
