using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;
using System;

namespace NL {
    [System.Serializable]
    public class PlayerMouseViewEntry : EntryBase {

        
        public  Position3Entry Position;
        
        
        public  Position3Entry Rotation;

        
        public string State;

        
        public float MakingAmountValue;

        
        public float MakingAmountMaxValue;

        
        public uint PlayerArrangementTargetId;
    }

    public interface IPlayerMouseViewRepository {
        PlayerMouseViewModel Get (uint id);
        List<PlayerMouseViewModel> GetAll ();
        PlayerMouseViewModel Create (Vector3 position, MakingAmount makingAmount, PlayerArrangementTargetModel playerArrangementTargetModel);
        void Store (PlayerMouseViewModel playerMouseViewModel);
        void Remove (PlayerMouseViewModel playerMouseViewModel);
    }

    /// <summary>
    /// マウスのストック情報に関する
    /// </summary>
    public class PlayerMouseViewRepository : PlayerRepositoryBase<PlayerMouseViewEntry>, IPlayerMouseViewRepository {

        private readonly IPlayerArrangementTargetRepository playerArrangementTargetRepository = null;

        public PlayerMouseViewRepository (IPlayerArrangementTargetRepository playerArrangementTargetRepository, PlayerContextMap playerContextMap) : base (playerContextMap.PlayerMouseViewEntrys) {
            this.playerArrangementTargetRepository = playerArrangementTargetRepository;
        }

        private PlayerMouseViewModel CreateByEntry (PlayerMouseViewEntry entry) {

            var playerArrangementTargetModel = this.playerArrangementTargetRepository.Get(entry.PlayerArrangementTargetId);
            Debug.Assert(playerArrangementTargetModel != null, "PlayerArrangementTargetModelが存在しません。:" + playerArrangementTargetModel.Id.ToString()) ;

            var state = MouseViewState.None;
            if (Enum.TryParse (entry.State, out MouseViewState outState)) {
                state = outState;
            }

            return new PlayerMouseViewModel(
                entry.Id,
                new Vector3(entry.Position.X, entry.Position.Y, entry.Position.Z),
                new Vector3(entry.Rotation.X, entry.Rotation.Y, entry.Rotation.Z),
                state,
                new MakingAmount(entry.MakingAmountValue, entry.MakingAmountMaxValue),
                playerArrangementTargetModel
            );
        }

        public PlayerMouseViewModel Get (uint id) {
            var foundEntry = this.GetEntry(id);
            if (foundEntry == null) {
                return null;
            }
            return CreateByEntry(foundEntry);
        }

        public List<PlayerMouseViewModel> GetAll () {
            return this.entrys.Select(entry => CreateByEntry(entry)).ToList();
        }

        public PlayerMouseViewModel Create (
            Vector3 position, 
            MakingAmount makingAmount, 
            PlayerArrangementTargetModel playerArrangementTargetModel
        ) {
            var id = this.MaximuId()+1;
            var entry = new PlayerMouseViewEntry () {
                Id = id,
                Position = new Position3Entry() {
                    X = position.x,
                    Y = position.y,
                    Z = position.z
                },
                Rotation = new Position3Entry() {
                    X = 0.0f,
                    Y = 0.0f,
                    Z = 0.0f
                },
                State = MouseViewState.None.ToString(),
                MakingAmountValue = makingAmount.Value,
                MakingAmountMaxValue = makingAmount.MaxValue,
                PlayerArrangementTargetId = playerArrangementTargetModel.Id
            };
            this.entrys.Add(entry);
            PlayerContextMap.WriteEntry (this.entrys);
            return CreateByEntry(entry);
        }

        public void Store (PlayerMouseViewModel playerMouseViewModel) {
            var entry = this.GetEntry(playerMouseViewModel.Id);
            if (entry != null) {
                var index = this.entrys.IndexOf (entry);
                this.entrys[index] = new PlayerMouseViewEntry () {
                    Id = playerMouseViewModel.Id,
                    Position = new Position3Entry() {
                        X = playerMouseViewModel.Position.x,
                        Y = playerMouseViewModel.Position.y,
                        Z = playerMouseViewModel.Position.z
                    },
                    Rotation = new Position3Entry() {
                        X = playerMouseViewModel.Rotation.x,
                        Y = playerMouseViewModel.Rotation.y,
                        Z = playerMouseViewModel.Rotation.z
                    },
                    State = playerMouseViewModel.State.ToString(),
                    MakingAmountValue = playerMouseViewModel.MakingAmount.Value,
                    MakingAmountMaxValue = playerMouseViewModel.MakingAmount.MaxValue,
                    PlayerArrangementTargetId = playerMouseViewModel.PlayerArrangementTargetModel.Id
                };
            } else {
                Debug.Assert(false,"要素が存在しません : " + playerMouseViewModel.Id.ToString());
            }
            PlayerContextMap.WriteEntry (this.entrys);            
        }

        public void Remove (PlayerMouseViewModel playerMouseViewModel) {
            var entry = this.GetEntry(playerMouseViewModel.Id);
            if (entry != null) {
                var index = this.entrys.IndexOf (entry);
                this.entrys.RemoveAt(index);
            } else {
                Debug.Assert(false,"要素が存在しません : " + playerMouseViewModel.Id.ToString());
            }
            PlayerContextMap.WriteEntry (this.entrys);            
        }
    }
}