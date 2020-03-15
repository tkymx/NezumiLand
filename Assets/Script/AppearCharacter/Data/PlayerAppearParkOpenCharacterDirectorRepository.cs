using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;
using System;

namespace NL {

    [System.Serializable]
    public class PlayerAppearParkOpenCharacterDirectorEntry : EntryBase 
    {        
        public uint AppearParkOpenCharacterDirectorId;
    }

    public interface IPlayerAppearParkOpenCharacterDirectorRepository 
    {
        PlayerAppearParkOpenCharacterDirectorModel Get (uint id);        
        List<PlayerAppearParkOpenCharacterDirectorModel> GetAll ();      
        PlayerAppearParkOpenCharacterDirectorModel Create (AppearParkOpenCharacterDirectorModel appearParkOpenCharacterDirectorModel);
        void Store (PlayerAppearParkOpenCharacterDirectorModel PlayerAppearParkOpenCharacterDirectorModel);
        void Remove (PlayerAppearParkOpenCharacterDirectorModel PlayerAppearParkOpenCharacterDirectorModel);
    }

    public class PlayerAppearParkOpenCharacterDirectorRepository : PlayerRepositoryBase<PlayerAppearParkOpenCharacterDirectorEntry>, IPlayerAppearParkOpenCharacterDirectorRepository {

        private readonly AppearParkOpenCharacterDirectorRepository appearParkOpenCharacterDirectorRepository;

        public PlayerAppearParkOpenCharacterDirectorRepository (AppearParkOpenCharacterDirectorRepository appearParkOpenCharacterDirectorRepository, PlayerContextMap playerContextMap) : base (playerContextMap.PlayerAppearParkOpenCharacterDirectorEntrys) {
            this.appearParkOpenCharacterDirectorRepository = appearParkOpenCharacterDirectorRepository;
        }

        private PlayerAppearParkOpenCharacterDirectorModel CreateByEntry (PlayerAppearParkOpenCharacterDirectorEntry entry) {

            var appearParkOpenCharacterDirectorModel = this.appearParkOpenCharacterDirectorRepository.Get(entry.AppearParkOpenCharacterDirectorId);
            Debug.Assert(appearParkOpenCharacterDirectorModel != null, "appearParkOpenCharacterDirectorModel が存在しません。");
                  
            return new PlayerAppearParkOpenCharacterDirectorModel (
                entry.Id,
                appearParkOpenCharacterDirectorModel);
        }

        public PlayerAppearParkOpenCharacterDirectorModel Get (uint id) {
            var foundEntry = this.GetEntry(id);
            if (foundEntry == null) {
                return null;
            }
            return CreateByEntry(foundEntry);
        }

        public List<PlayerAppearParkOpenCharacterDirectorModel> GetAll () {
            return this.entrys.Select(entry => CreateByEntry(entry)).ToList();
        }

        public PlayerAppearParkOpenCharacterDirectorModel Create (AppearParkOpenCharacterDirectorModel appearParkOpenCharacterDirectorModel)
        {
            var id = this.MaximuId()+1;

            var entry = new PlayerAppearParkOpenCharacterDirectorEntry () {
                Id = id,
                AppearParkOpenCharacterDirectorId = appearParkOpenCharacterDirectorModel.Id
            };
            this.entrys.Add(entry);
            PlayerContextMap.WriteEntry (this.entrys);
            return CreateByEntry(entry);
        }

        public void Store (PlayerAppearParkOpenCharacterDirectorModel PlayerAppearParkOpenCharacterDirectorModel) {
            var entry = this.GetEntry(PlayerAppearParkOpenCharacterDirectorModel.Id);
            if (entry != null) {
                var index = this.entrys.IndexOf (entry);
                this.entrys[index] = new PlayerAppearParkOpenCharacterDirectorEntry () {
                    Id = PlayerAppearParkOpenCharacterDirectorModel.Id,
                    AppearParkOpenCharacterDirectorId = PlayerAppearParkOpenCharacterDirectorModel.appearParkOpenCharacterDirectorModel.Id,
                };
            } else {
                Debug.Assert(false,"要素が存在しません : " + PlayerAppearParkOpenCharacterDirectorModel.Id.ToString());
            }
            PlayerContextMap.WriteEntry (this.entrys);            
        }

        public void Remove (PlayerAppearParkOpenCharacterDirectorModel PlayerAppearParkOpenCharacterDirectorModel) {
            var entry = this.GetEntry(PlayerAppearParkOpenCharacterDirectorModel.Id);
            if (entry != null) {
                var index = this.entrys.IndexOf (entry);
                this.entrys.RemoveAt(index);
            } else {
                Debug.Assert(false,"要素が存在しません : " + PlayerAppearParkOpenCharacterDirectorModel.Id.ToString());
            }
            PlayerContextMap.WriteEntry (this.entrys);            
        }
    }
}
