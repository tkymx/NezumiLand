using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace NL {

    [System.Serializable]
    public class PlayerParkOpenGroupEntry : EntryBase 
    {
        public uint ParkOpenGroupId;
        public bool IsOpen;
        public bool IsClear;
        public string CurrentOpenType;
    }

    public interface IPlayerParkOpenGroupRepository 
    {
        PlayerParkOpenGroupModel Get (uint id);
        List<PlayerParkOpenGroupModel> GetDisplayale (ParkOpenGroupsModel parkOpenGroupsModel);
        void Store (PlayerParkOpenGroupModel playerParkOpenGroupModel);
        void Store (List<PlayerParkOpenGroupModel> playerParkOpenGroupModels);
        void Remove (PlayerParkOpenGroupModel playerParkOpenGroupModel);
    }

    public class PlayerParkOpenGroupRepository : PlayerRepositoryBase<PlayerParkOpenGroupEntry>, IPlayerParkOpenGroupRepository {

        private IParkOpenGroupRepository parkOpenGroupRepository;

        public PlayerParkOpenGroupRepository (IParkOpenGroupRepository parkOpenGroupRepository, PlayerContextMap playerContextMap) : base (playerContextMap.PlayerParkOpenGroupEntrys) {
            this.parkOpenGroupRepository = parkOpenGroupRepository;
        }

        private PlayerParkOpenGroupModel CreateByEntry (PlayerParkOpenGroupEntry entry) {

            var parkOpenGroupModel = this.parkOpenGroupRepository.Get(entry.ParkOpenGroupId);
            Debug.Assert(parkOpenGroupModel!=null, "parkOpenGroupModel が存在していません");

            var openType = PlayerParkOpenGroupModel.OpenType.Normal;
            if (Enum.TryParse (entry.CurrentOpenType, out PlayerParkOpenGroupModel.OpenType outState)) {
                openType = outState;
            }

            return new PlayerParkOpenGroupModel(
                entry.Id,
                parkOpenGroupModel,
                entry.IsOpen,
                entry.IsClear,
                openType
            );
        }

        public PlayerParkOpenGroupModel Get (uint id) {
            var foundEntry = this.GetEntry(id);
            if (foundEntry == null) {

                var parkOpenGroupModel = this.parkOpenGroupRepository.Get(id);
                Debug.Assert(parkOpenGroupModel!=null, "parkOpenGroupModel が存在していません");

                return this.Create(parkOpenGroupModel);
            }
            return CreateByEntry(foundEntry);
        }

        public List<PlayerParkOpenGroupModel> GetDisplayale (ParkOpenGroupsModel parkOpenGroupsModel)
        {
            return parkOpenGroupsModel.ParkOpenGroupModels
                .Select(parkOpenGroupModel => this.Get(parkOpenGroupModel.Id))
                .Where(playerParkOpenGroupModel => playerParkOpenGroupModel.IsOpen )
                .ToList();
        }

        private PlayerParkOpenGroupModel Create (ParkOpenGroupModel parkOpenGroupModel) 
        {
            var id = parkOpenGroupModel.Id;

            var entry = new PlayerParkOpenGroupEntry () {
                Id = id,
                ParkOpenGroupId = parkOpenGroupModel.Id,
                IsOpen = parkOpenGroupModel.IsInitialOpen ? true : false,
                IsClear = false,
                CurrentOpenType = PlayerParkOpenGroupModel.OpenType.Normal.ToString()
            };
            this.entrys.Add(entry);
            PlayerContextMap.WriteEntry (this.entrys);
            return CreateByEntry(entry);
        }

        public void Store (PlayerParkOpenGroupModel playerParkOpenGroupModel) {
            var entry = this.GetEntry(playerParkOpenGroupModel.Id);
            if (entry != null) {
                var index = this.entrys.IndexOf (entry);
                this.entrys[index] = new PlayerParkOpenGroupEntry () {
                    Id = playerParkOpenGroupModel.Id,
                    ParkOpenGroupId = playerParkOpenGroupModel.ParkOpenGroupModel.Id,
                    IsOpen = playerParkOpenGroupModel.IsOpen,
                    IsClear = playerParkOpenGroupModel.IsClear,
                    CurrentOpenType = playerParkOpenGroupModel.CurrentOpenType.ToString()
                };
            } else {
                Debug.Assert(false,"要素が存在しません : " + playerParkOpenGroupModel.Id.ToString());
            }
            PlayerContextMap.WriteEntry (this.entrys);            
        }

        public void Store (List<PlayerParkOpenGroupModel> playerParkOpenGroupModels) {
            foreach (var playerParkOpenGroupModel in playerParkOpenGroupModels)
            {
                var entry = this.GetEntry(playerParkOpenGroupModel.Id);
                if (entry != null) {
                    var index = this.entrys.IndexOf (entry);
                    this.entrys[index] = new PlayerParkOpenGroupEntry () {
                        Id = playerParkOpenGroupModel.Id,
                        ParkOpenGroupId = playerParkOpenGroupModel.ParkOpenGroupModel.Id,
                        IsOpen = playerParkOpenGroupModel.IsOpen,
                        IsClear = playerParkOpenGroupModel.IsClear,
                        CurrentOpenType = playerParkOpenGroupModel.CurrentOpenType.ToString()
                    };
                } else {
                    Debug.Assert(false,"要素が存在しません : " + playerParkOpenGroupModel.Id.ToString());
                }                
            }
            PlayerContextMap.WriteEntry (this.entrys);            
        }        

        public void Remove (PlayerParkOpenGroupModel playerParkOpenGroupModel) {
            var entry = this.GetEntry(playerParkOpenGroupModel.Id);
            if (entry != null) {
                var index = this.entrys.IndexOf (entry);
                this.entrys.RemoveAt(index);
            } else {
                Debug.Assert(false,"要素が存在しません : " + playerParkOpenGroupModel.Id.ToString());
            }
            PlayerContextMap.WriteEntry (this.entrys);            
        }
    }
}
