using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;
using System;

namespace NL {

    [System.Serializable]
    public class PlayerAppearCharacterViewEntry : EntryBase 
    {        
        public Position3Entry Position;
        public Position3Entry Rotation;
        public uint PlayerAppearCharacterReserveId;
        public bool IsReceiveReward;
        public string AppearCharacterState;
        public uint PlayerArrangementTargetId;
    }

    public interface IPlayerAppearCharacterViewRepository 
    {
        PlayerAppearCharacterViewModel Get (uint id);        
        List<PlayerAppearCharacterViewModel> GetAll ();      
        PlayerAppearCharacterViewModel Create (           
            Vector3 position,
            Vector3 rotation,
            PlayerAppearCharacterReserveModel playerAppearCharacterReserveModel);
        void Store (PlayerAppearCharacterViewModel playerAppearCharacterViewModel);
        void Remove (PlayerAppearCharacterViewModel playerAppearCharacterViewModel);
    }

    public class PlayerAppearCharacterViewRepository : PlayerRepositoryBase<PlayerAppearCharacterViewEntry>, IPlayerAppearCharacterViewRepository {

        private readonly IPlayerAppearCharacterReserveRepository playerAppearCharacterReserveRepository;
        private readonly IPlayerArrangementTargetRepository playerArrangementTargetRepository;

        public PlayerAppearCharacterViewRepository (IPlayerAppearCharacterReserveRepository playerAppearCharacterReserveRepository, IPlayerArrangementTargetRepository playerArrangementTargetRepository, PlayerContextMap playerContextMap) : base (playerContextMap.PlayerAppearCharacterViewEntrys) {
            this.playerAppearCharacterReserveRepository = playerAppearCharacterReserveRepository;
            this.playerArrangementTargetRepository = playerArrangementTargetRepository;
        }

        private PlayerAppearCharacterViewModel CreateByEntry (PlayerAppearCharacterViewEntry entry) {

            var playerAppearCharacterReserveModel = this.playerAppearCharacterReserveRepository.Get(entry.PlayerAppearCharacterReserveId);
            Debug.Assert(playerAppearCharacterReserveModel!=null, "playerAppearCharacterReserveModel が存在していません");
            
            var playerArrangementTargetModel = this.playerArrangementTargetRepository.Get(entry.PlayerArrangementTargetId);
            
            var state = AppearCharacterState.None;
            if (Enum.TryParse (entry.AppearCharacterState, out AppearCharacterState outState)) {
                state = outState;
            }

            return new PlayerAppearCharacterViewModel(
                entry.Id,
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
                playerArrangementTargetModel
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
            Vector3 position,
            Vector3 rotation,
            PlayerAppearCharacterReserveModel playerAppearCharacterReserveModel
        ) {
            var id = this.MaximuId()+1;

            var entry = new PlayerAppearCharacterViewEntry () {
                Id = id,
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
                PlayerAppearCharacterReserveId = playerAppearCharacterReserveModel.Id,
                IsReceiveReward = false /*はじめはまだ受け取っていない*/,
                AppearCharacterState = AppearCharacterState.None.ToString(),
                PlayerArrangementTargetId = 0
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
                    PlayerAppearCharacterReserveId = playerAppearCharacterViewModel.PlayerAppearCharacterReserveModel.Id,
                    IsReceiveReward = playerAppearCharacterViewModel.IsReceiveReward,
                    AppearCharacterState = playerAppearCharacterViewModel.AppearCharacterState.ToString(),
                    PlayerArrangementTargetId = playerAppearCharacterViewModel.PlayerArrangementTargetModel.Id
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
