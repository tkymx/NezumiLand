using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;
using System;

namespace NL {

    [System.Serializable]
    public class Position3Entry {
        
        public float X;
        
        public float Y;
        
        public float Z;

        public Vector3 ToVector3 () {
            return new Vector3(X,Y,Z);
        }
        public static Position3Entry FromVector3 (Vector3 position) {
            return new Position3Entry () {
                X = position.x,
                Y = position.y,
                Z = position.z
            };
        }
    }

    [System.Serializable]
    public class Position2Entry {
        
        public int X;
        
        public int Z;

        public Vector2 ToVector2 () {
            return new Vector2(X,Z);
        }
      
        public Vector3 ToVector3 () {
            return new Vector3(X,0,Z);
        }
    }    

    [System.Serializable]
    public class PlayerArrangementTargetEntry : EntryBase {

        
        public Position3Entry CenterPosition;

        
        public float Range;

        
        public Position2Entry[] ArrangementPositions;

        
        public string ArrangementTargetState;

        
        public bool HasMonoInfoId;

        
        public uint MonoInfoId;

        
        public bool HasMonoViewModelId;

        
        public uint MonoViewModelId;
    }

    public interface IPlayerArrangementTargetRepository {
        PlayerArrangementTargetModel Get (uint id);        
        List<PlayerArrangementTargetModel> GetAll ();      
        PlayerArrangementTargetModel Create (           
            Vector3 centerPosition,
            float range,
            List<ArrangementPosition> positions,
            ArrangementTargetState state,
            MonoInfo monoInfo,
            PlayerMonoViewModel playerMonoViewModel);
        void Store (PlayerArrangementTargetModel playerArrangementTargetModel);
        void Remove (PlayerArrangementTargetModel playerArrangementTargetModel);
    }

    public class PlayerArrangementTargetRepository : PlayerRepositoryBase<PlayerArrangementTargetEntry>, IPlayerArrangementTargetRepository {
        private readonly IMonoInfoRepository monoInfoRepository;
        private readonly IPlayerMonoViewRepository playerMonoViewRepository;

        public PlayerArrangementTargetRepository (IMonoInfoRepository monoInfoRepository, IPlayerMonoViewRepository playerMonoViewRepository, PlayerContextMap playerContextMap) : base (playerContextMap.PlayerArrangementTargetEntrys) {
            this.monoInfoRepository = monoInfoRepository;
            this.playerMonoViewRepository = playerMonoViewRepository;
        }

        private PlayerArrangementTargetModel CreateByEntry (PlayerArrangementTargetEntry entry) {

            MonoInfo monoInfo = null;
            if (entry.HasMonoInfoId) {
                monoInfo = this.monoInfoRepository.Get(entry.MonoInfoId);
                Debug.Assert(monoInfo!=null, "MonoInfoが存在していません");
            }
            PlayerMonoViewModel monoViewModel = null;
            if (entry.HasMonoViewModelId) {
                monoViewModel = this.playerMonoViewRepository.Get(entry.MonoViewModelId);
                Debug.Assert(monoViewModel!=null, "MonoViewModelが存在していません");
            }

            var state = ArrangementTargetState.None;
            if (Enum.TryParse (entry.ArrangementTargetState, out ArrangementTargetState outState)) {
                state = outState;
            }

            return new PlayerArrangementTargetModel(
                entry.Id,
                new Vector3(entry.CenterPosition.X, entry.CenterPosition.Y, entry.CenterPosition.Z),
                entry.Range,
                entry.ArrangementPositions.Select(pos => new ArrangementPosition(){
                    x = pos.X,
                    z = pos.Z,           
                }).ToList(),
                state,
                entry.HasMonoInfoId ? monoInfo : null,
                entry.HasMonoViewModelId ? monoViewModel : null
            );
        }

        public PlayerArrangementTargetModel Get (uint id) {
            var foundEntry = this.GetEntry(id);
            if (foundEntry == null) {
                return null;
            }
            return CreateByEntry(foundEntry);
        }

        public List<PlayerArrangementTargetModel> GetAll () {
            return this.entrys.Select(entry => CreateByEntry(entry)).ToList();
        }

        public PlayerArrangementTargetModel Create (
            Vector3 centerPosition,
            float range,
            List<ArrangementPosition> positions,
            ArrangementTargetState state,
            MonoInfo monoInfo,
            PlayerMonoViewModel playerMonoViewModel
        ) {
            var id = this.MaximuId()+1;
            var entry = new PlayerArrangementTargetEntry () {
                Id = id,
                CenterPosition = new Position3Entry() {
                    X = centerPosition.x,
                    Y = centerPosition.y,
                    Z = centerPosition.z
                },
                Range = range,
                ArrangementPositions = positions.Select(pos => new Position2Entry(){
                    X = pos.x,
                    Z = pos.z
                }).ToArray(),
                ArrangementTargetState = state.ToString(),
                HasMonoInfoId = monoInfo != null,
                MonoInfoId = monoInfo != null ? monoInfo.Id : uint.MaxValue,
                HasMonoViewModelId = playerMonoViewModel != null,
                MonoViewModelId = playerMonoViewModel != null ? playerMonoViewModel.Id : uint.MaxValue
            };
            this.entrys.Add(entry);
            PlayerContextMap.WriteEntry (this.entrys);
            return CreateByEntry(entry);
        }

        public void Store (PlayerArrangementTargetModel playerArrangementTargetModel) {
            var entry = this.GetEntry(playerArrangementTargetModel.Id);
            if (entry != null) {
                var index = this.entrys.IndexOf (entry);
                this.entrys[index] = new PlayerArrangementTargetEntry () {
                    Id = playerArrangementTargetModel.Id,
                    CenterPosition = new Position3Entry() {
                        X = playerArrangementTargetModel.CenterPosition.x,
                        Y = playerArrangementTargetModel.CenterPosition.y,
                        Z = playerArrangementTargetModel.CenterPosition.z
                    },
                    Range = playerArrangementTargetModel.Range,
                    ArrangementPositions = playerArrangementTargetModel.Positions.Select(pos => new Position2Entry(){
                        X = pos.x,
                        Z = pos.z
                    }).ToArray(),
                    ArrangementTargetState = playerArrangementTargetModel.State.ToString(),
                    HasMonoInfoId = playerArrangementTargetModel.MonoInfo != null,
                    MonoInfoId = playerArrangementTargetModel.MonoInfo != null ? playerArrangementTargetModel.MonoInfo.Id : uint.MaxValue,
                    HasMonoViewModelId = playerArrangementTargetModel.PlayerMonoViewModel != null,
                    MonoViewModelId = playerArrangementTargetModel.PlayerMonoViewModel != null ? playerArrangementTargetModel.PlayerMonoViewModel.Id : uint.MaxValue
                };
            } else {
                Debug.Assert(false,"要素が存在しません : " + playerArrangementTargetModel.Id.ToString());
            }
            PlayerContextMap.WriteEntry (this.entrys);            
        }

        public void Remove (PlayerArrangementTargetModel playerArrangementTargetModel) {
            var entry = this.GetEntry(playerArrangementTargetModel.Id);
            if (entry != null) {
                var index = this.entrys.IndexOf (entry);
                this.entrys.RemoveAt(index);
            } else {
                Debug.Assert(false,"要素が存在しません : " + playerArrangementTargetModel.Id.ToString());
            }
            PlayerContextMap.WriteEntry (this.entrys);            
        }
    }
}
