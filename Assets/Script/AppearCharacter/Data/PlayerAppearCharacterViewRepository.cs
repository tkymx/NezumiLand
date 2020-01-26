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
        public MovePathEntry MovePath;
    }

    public interface IPlayerAppearCharacterViewRepository 
    {
        PlayerAppearCharacterViewModel Get (uint id);        
        List<PlayerAppearCharacterViewModel> GetAll ();      
        PlayerAppearCharacterViewModel Create (  
            AppearCharacterModel appearCharacterModel,         
            Vector3 position,
            Vector3 rotation,            
            AppearCharacterLifeDirectorType AppearCharacterLifeDirectorType,
            PlayerAppearCharacterReserveModel playerAppearCharacterReserveModel,
            MovePath movePath);
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
                entry.Position.ToVector3(),
                entry.Rotation.ToVector3(),
                playerAppearCharacterReserveModel,
                entry.IsReceiveReward,
                state,
                playerArrangementTargetModel,
                entry.CurrentPlayingTime,
                type,
                new MovePath(
                    entry.MovePath.AppearPosition.ToVector3(),
                    entry.MovePath.DisappearPosition.ToVector3()
                )
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
            AppearCharacterLifeDirectorType appearCharacterLifeDirectorType,
            PlayerAppearCharacterReserveModel playerAppearCharacterReserveModel,
            MovePath movePath
        ) {
            var id = this.MaximuId()+1;

            var entry = new PlayerAppearCharacterViewEntry () {
                Id = id,
                AppearCharacterId = appearCharacterModel.Id,
                Position = Position3Entry.FromVector3(position),
                Rotation = Position3Entry.FromVector3(rotation),
                PlayerAppearCharacterReserveId = playerAppearCharacterReserveModel != null ? playerAppearCharacterReserveModel.Id : 0,
                IsReceiveReward = false /*はじめはまだ受け取っていない*/,
                AppearCharacterState = AppearCharacterState.None.ToString(),
                PlayerArrangementTargetId = 0,
                CurrentPlayingTime = 0,
                AppearCharacterLifeDirectorType = appearCharacterLifeDirectorType.ToString(),
                MovePath = new MovePathEntry () {
                    AppearPosition = Position3Entry.FromVector3(movePath.AppearPosition),
                    DisappearPosition = Position3Entry.FromVector3(movePath.DisapearPosition)
                }
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
                    Position = Position3Entry.FromVector3(playerAppearCharacterViewModel.Position),
                    Rotation = Position3Entry.FromVector3(playerAppearCharacterViewModel.Rotation),
                    PlayerAppearCharacterReserveId = playerAppearCharacterViewModel.PlayerAppearCharacterReserveModelInDirector != null ? playerAppearCharacterViewModel.PlayerAppearCharacterReserveModelInDirector.Id : 0,
                    IsReceiveReward = playerAppearCharacterViewModel.IsReceiveReward,
                    AppearCharacterState = playerAppearCharacterViewModel.AppearCharacterState.ToString(),
                    PlayerArrangementTargetId = playerAppearCharacterViewModel.PlayerArrangementTargetModel.Id,
                    CurrentPlayingTime = playerAppearCharacterViewModel.CurrentPlayingTime,
                    AppearCharacterLifeDirectorType = playerAppearCharacterViewModel.AppearCharacterLifeDirectorType.ToString(),
                    MovePath = new MovePathEntry () {
                        AppearPosition = Position3Entry.FromVector3(playerAppearCharacterViewModel.MovePath.AppearPosition),
                        DisappearPosition = Position3Entry.FromVector3(playerAppearCharacterViewModel.MovePath.DisapearPosition)
                    }                    
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
