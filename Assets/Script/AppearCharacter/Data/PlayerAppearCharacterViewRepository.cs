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

        // state 
        public Position3Entry Position;
        public Position3Entry Rotation;
        public string AppearCharacterState;
        public float CurrentPlayingTime;
        public bool HasPlayerArrangementTarget;
        public uint PlayerArrangementTargetId;
        public MovePathEntry MovePath;

        // director
        public string AppearCharacterLifeDirectorType;
        public uint PlayerAppearCharacterLifeDirectorModelId;
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
            PlayerAppearCharacterDirectorModelBase PlayerAppearCharacterDirectorModel,
            MovePath movePath);
        void Store (PlayerAppearCharacterViewModel playerAppearCharacterViewModel);
        void Remove (PlayerAppearCharacterViewModel playerAppearCharacterViewModel);
    }

    public class PlayerAppearCharacterViewRepository : PlayerRepositoryBase<PlayerAppearCharacterViewEntry>, IPlayerAppearCharacterViewRepository {

        private readonly AppearCharacterRepository appearCharacterRepository;
        private readonly IPlayerAppearCharacterReserveRepository playerAppearCharacterReserveRepository;
        private readonly IPlayerArrangementTargetRepository playerArrangementTargetRepository;

        // Director
        private readonly IPlayerAppearConversationCharacterDirectorRepository playerAppearConversationCharacterDirectorRepository;
        private readonly IPlayerAppearOnegaiCharacterDirectorRepository playerAppearOnegaiCharacterDirectorRepository;
        private readonly IPlayerAppearParkOpenCharacterDirectorRepository playerAppearParkOpenCharacterDirectorRepository;

        public PlayerAppearCharacterViewRepository (
            AppearCharacterRepository appearCharacterRepository, 
            IPlayerAppearCharacterReserveRepository playerAppearCharacterReserveRepository,  
            IPlayerArrangementTargetRepository playerArrangementTargetRepository, 
            IPlayerAppearConversationCharacterDirectorRepository playerAppearConversationCharacterDirectorRepository,
            IPlayerAppearOnegaiCharacterDirectorRepository playerAppearOnegaiCharacterDirectorRepository,
            IPlayerAppearParkOpenCharacterDirectorRepository playerAppearParkOpenCharacterDirectorRepository,
            PlayerContextMap playerContextMap) : base (playerContextMap.PlayerAppearCharacterViewEntrys)     
        {
            this.appearCharacterRepository = appearCharacterRepository;
            this.playerAppearCharacterReserveRepository = playerAppearCharacterReserveRepository;
            this.playerArrangementTargetRepository = playerArrangementTargetRepository;         
            this.playerAppearConversationCharacterDirectorRepository = playerAppearConversationCharacterDirectorRepository;
            this.playerAppearOnegaiCharacterDirectorRepository = playerAppearOnegaiCharacterDirectorRepository;
            this.playerAppearParkOpenCharacterDirectorRepository = playerAppearParkOpenCharacterDirectorRepository;   
        }

        private PlayerAppearCharacterViewModel CreateByEntry (PlayerAppearCharacterViewEntry entry) {

            var appearCharacterModel = this.appearCharacterRepository.Get(entry.AppearCharacterId);      
            Debug.Assert(appearCharacterModel != null, "appearCharacterModelが存在しません。");

            var state = AppearCharacterState.None;
            if (Enum.TryParse (entry.AppearCharacterState, out AppearCharacterState outState)) {
                state = outState;
            }

            var type = AppearCharacterLifeDirectorType.None;
            if (Enum.TryParse (entry.AppearCharacterLifeDirectorType, out AppearCharacterLifeDirectorType outType)) {
                type = outType;
            }

            PlayerArrangementTargetModel playerArrangementTargetModel = null;
            if (entry.HasPlayerArrangementTarget) {
                playerArrangementTargetModel = this.playerArrangementTargetRepository.Get(entry.PlayerArrangementTargetId);
                Debug.Assert(playerArrangementTargetModel != null, "playerArrangementTargetModel が存在しません。");
            }

            PlayerAppearCharacterDirectorModelBase directorModel = null;
            if (type == AppearCharacterLifeDirectorType.Conversation) 
            {
                directorModel = this.playerAppearConversationCharacterDirectorRepository.Get(entry.PlayerAppearCharacterLifeDirectorModelId);
                Debug.Assert(directorModel != null, "PlayerAppearConversationCharacterDirector が存在しません。");
            }
            else if (type == AppearCharacterLifeDirectorType.ParkOpen)
            {
                directorModel = this.playerAppearParkOpenCharacterDirectorRepository.Get(entry.PlayerAppearCharacterLifeDirectorModelId);
                Debug.Assert(directorModel != null, "PlayerAppearParkOpenCharacterDirector が存在しません。");
            }
            else if (type == AppearCharacterLifeDirectorType.Onegai)
            {
                directorModel = this.playerAppearOnegaiCharacterDirectorRepository.Get(entry.PlayerAppearCharacterLifeDirectorModelId);
                Debug.Assert(directorModel != null, "playerAppearOnegaiCharacterDirector が存在しません ");
            }
            else 
            {
                Debug.Assert(false, "未確認のタイプです。" + type.ToString() );
            }

            return new PlayerAppearCharacterViewModel(
                entry.Id,
                appearCharacterModel,
                entry.Position.ToVector3(),
                entry.Rotation.ToVector3(),
                state,
                entry.CurrentPlayingTime,
                playerArrangementTargetModel,
                type,
                directorModel,
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
            PlayerAppearCharacterDirectorModelBase playerAppearCharacterDirectorModel,
            MovePath movePath
        ) {
            var id = this.MaximuId()+1;

            var entry = new PlayerAppearCharacterViewEntry () {
                Id = id,
                AppearCharacterId = appearCharacterModel.Id,
                Position = Position3Entry.FromVector3(position),
                Rotation = Position3Entry.FromVector3(rotation),
                AppearCharacterState = AppearCharacterState.None.ToString(),
                CurrentPlayingTime = 0,
                HasPlayerArrangementTarget = false,
                PlayerArrangementTargetId = 0,
                AppearCharacterLifeDirectorType = appearCharacterLifeDirectorType.ToString(),
                PlayerAppearCharacterLifeDirectorModelId = playerAppearCharacterDirectorModel.Id,
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
                    AppearCharacterState = playerAppearCharacterViewModel.AppearCharacterState.ToString(),
                    CurrentPlayingTime = playerAppearCharacterViewModel.CurrentPlayingTime,
                    HasPlayerArrangementTarget = playerAppearCharacterViewModel.PlayerArrangementTargetModel != null,
                    PlayerArrangementTargetId = playerAppearCharacterViewModel.PlayerArrangementTargetModel != null ? playerAppearCharacterViewModel.PlayerArrangementTargetModel.Id : 0,
                    AppearCharacterLifeDirectorType = playerAppearCharacterViewModel.AppearCharacterLifeDirectorType.ToString(),
                    PlayerAppearCharacterLifeDirectorModelId = playerAppearCharacterViewModel.PlayerAppearCharacterDirectorModelBase.Id,
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
