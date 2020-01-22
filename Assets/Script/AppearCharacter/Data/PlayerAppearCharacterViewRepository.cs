using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;
using System;

namespace NL {

    [System.Serializable]
    public class PlayerAppearCharacterViewEntry : EntryBase 
    {        
        public uint AppearCharacterId;
        public Position3Entry Position;
        public Position3Entry Rotation;
        public uint PlayerAppearCharacterReserveId;
        public bool IsReceiveReward;
        public string AppearCharacterState;
        public uint PlayerArrangementTargetId;
        public float CurrentPlayingTime;
        public string AppearCharacterLifeDirectorType;
    }

    public interface IPlayerAppearCharacterViewRepository 
    {
        PlayerAppearCharacterViewModel Get (uint id);        
        List<PlayerAppearCharacterViewModel> GetAll ();      
        PlayerAppearCharacterViewModel Create (  
            AppearCharacterModel appearCharacterModel,         
            Vector3 position,
            Vector3 rotation,
            PlayerAppearCharacterReserveModel playerAppearCharacterReserveModel,
            AppearCharacterLifeDirectorType AppearCharacterLifeDirectorType);
        void Store (PlayerAppearCharacterViewModel playerAppearCharacterViewModel);
        void Remove (PlayerAppearCharacterViewModel playerAppearCharacterViewModel);
    }

    public class PlayerAppearCharacterViewRepository : PlayerRepositoryBase<PlayerAppearCharacterViewEntry>, IPlayerAppearCharacterViewRepository {

        private readonly AppearCharacterRepository appearCharacterRepository;
        private readonly IPlayerAppearCharacterReserveRepository playerAppearCharacterReserveRepository;
        private readonly IPlayerArrangementTargetRepository playerArrangementTargetRepository;

        public PlayerAppearCharacterViewRepository (AppearCharacterRepository appearCharacterRepository, IPlayerAppearCharacterReserveRepository playerAppearCharacterReserveRepository, IPlayerArrangementTargetRepository playerArrangementTargetRepository, PlayerContextMap playerContextMap) : base (playerContextMap.PlayerAppearCharacterViewEntrys) {
            this.appearCharacterRepository = appearCharacterRepository;
            this.playerAppearCharacterReserveRepository = playerAppearCharacterReserveRepository;
            this.playerArrangementTargetRepository = playerArrangementTargetRepository;
        }

        private PlayerAppearCharacterViewModel CreateByEntry (PlayerAppearCharacterViewEntry entry) {

            var appearCharacterModel = this.appearCharacterRepository.Get(entry.AppearCharacterId);      
            Debug.Assert(appearCharacterModel != null, "appearCharacterModelが存在しません。");
                  
            var playerAppearCharacterReserveModel = this.playerAppearCharacterReserveRepository.Get(entry.PlayerAppearCharacterReserveId);            
            var playerArrangementTargetModel = this.playerArrangementTargetRepository.Get(entry.PlayerArrangementTargetId);
            
            var state = AppearCharacterState.None;
            if (Enum.TryParse (entry.AppearCharacterState, out AppearCharacterState outState)) {
                state = outState;
            }

            var type = AppearCharacterLifeDirectorType.None;
            if (Enum.TryParse (entry.AppearCharacterLifeDirectorType, out AppearCharacterLifeDirectorType outType)) {
                type = outType;
            }

            return new PlayerAppearCharacterViewModel(
                entry.Id,
                appearCharacterModel,
                new Vector3(
                    entry.Position.X,
                    entry.Position.Y,
                    entry.Position.Z
                ),
                new Vector3(
                    entry.Rotation.X,
                    entry.Rotation.Y,
                    entry.Rotation.Z
                ),
                playerAppearCharacterReserveModel,
                entry.IsReceiveReward,
                state,
                playerArrangementTargetModel,
                entry.CurrentPlayingTime,
                type
            );
        }

        public PlayerAppearCharacterViewModel Get (uint id) {
            var foundEntry = this.GetEntry(id);
            if (foundEntry == null) {
                return null;
            }
            return CreateByEntry(foundEntry);
        }

        public List<PlayerAppearCharacterViewModel> GetAll () {
            return this.entrys.Select(entry => CreateByEntry(entry)).ToList();
        }

        public PlayerAppearCharacterViewModel Create (
            AppearCharacterModel appearCharacterModel,
            Vector3 position,
            Vector3 rotation,
            PlayerAppearCharacterReserveModel playerAppearCharacterReserveModel,
            AppearCharacterLifeDirectorType appearCharacterLifeDirectorType
        ) {
            var id = this.MaximuId()+1;

            var entry = new PlayerAppearCharacterViewEntry () {
                Id = id,
                AppearCharacterId = appearCharacterModel.Id,
                Position = new Position3Entry() {
                    X = position.x,
                    Y = position.y,
                    Z = position.z,
                },
                Rotation = new Position3Entry() {
                    X = rotation.x,
                    Y = rotation.y,
                    Z = rotation.z,
                },
                PlayerAppearCharacterReserveId = playerAppearCharacterReserveModel != null ? playerAppearCharacterReserveModel.Id : 0,
                IsReceiveReward = false /*はじめはまだ受け取っていない*/,
                AppearCharacterState = AppearCharacterState.None.ToString(),
                PlayerArrangementTargetId = 0,
                CurrentPlayingTime = 0,
                AppearCharacterLifeDirectorType = appearCharacterLifeDirectorType.ToString()
            };
            this.entrys.Add(entry);
            PlayerContextMap.WriteEntry (this.entrys);
            return CreateByEntry(entry);
        }

        public void Store (PlayerAppearCharacterViewModel playerAppearCharacterViewModel) {
            var entry = this.GetEntry(playerAppearCharacterViewModel.Id);
            if (entry != null) {
                var index = this.entrys.IndexOf (entry);
                this.entrys[index] = new PlayerAppearCharacterViewEntry () {
                    Id = playerAppearCharacterViewModel.Id,
                    AppearCharacterId = playerAppearCharacterViewModel.AppearCharacterModel.Id,
                    Position = new Position3Entry() {
                        X = playerAppearCharacterViewModel.Position.x,
                        Y = playerAppearCharacterViewModel.Position.y,
                        Z = playerAppearCharacterViewModel.Position.z,
                    },
                    Rotation = new Position3Entry() {
                        X = playerAppearCharacterViewModel.Rotation.x,
                        Y = playerAppearCharacterViewModel.Rotation.y,
                        Z = playerAppearCharacterViewModel.Rotation.z,
                    },
                    PlayerAppearCharacterReserveId = playerAppearCharacterViewModel.PlayerAppearCharacterReserveModelInDirector != null ? playerAppearCharacterViewModel.PlayerAppearCharacterReserveModelInDirector.Id : 0,
                    IsReceiveReward = playerAppearCharacterViewModel.IsReceiveReward,
                    AppearCharacterState = playerAppearCharacterViewModel.AppearCharacterState.ToString(),
                    PlayerArrangementTargetId = playerAppearCharacterViewModel.PlayerArrangementTargetModel.Id,
                    CurrentPlayingTime = playerAppearCharacterViewModel.CurrentPlayingTime,
                    AppearCharacterLifeDirectorType = playerAppearCharacterViewModel.AppearCharacterLifeDirectorType.ToString()
                };
            } else {
                Debug.Assert(false,"要素が存在しません : " + playerAppearCharacterViewModel.Id.ToString());
            }
            PlayerContextMap.WriteEntry (this.entrys);            
        }

        public void Remove (PlayerAppearCharacterViewModel playerAppearCharacterViewModel) {
            var entry = this.GetEntry(playerAppearCharacterViewModel.Id);
            if (entry != null) {
                var index = this.entrys.IndexOf (entry);
                this.entrys.RemoveAt(index);
            } else {
                Debug.Assert(false,"要素が存在しません : " + playerAppearCharacterViewModel.Id.ToString());
            }
            PlayerContextMap.WriteEntry (this.entrys);            
        }
    }
}
