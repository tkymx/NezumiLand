using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

namespace NL {

    [System.Serializable]
    public class ParkOpenGroupsEntry : EntryBase {
        public string Type;
        public string BackgroundSpriteName;
        public uint[] ParkOpenGroupIds;
    }

    public interface IParkOpenGroupsRepository {
        IEnumerable<ParkOpenGroupsModel> GetAll ();
        ParkOpenGroupsModel Get (uint id);
        ParkOpenGroupsModel GetByType (ParkOpenGroupsType type);
    }

    public class ParkOpenGroupsRepository : RepositoryBase<ParkOpenGroupsEntry>, IParkOpenGroupsRepository {

        private readonly IParkOpenGroupRepository parkOpenGroupRepository = null;

        public ParkOpenGroupsRepository (IParkOpenGroupRepository parkOpenGroupRepository, ContextMap contextMap) : base (contextMap.ParkOpenGroupsEntrys) 
        {
            this.parkOpenGroupRepository = parkOpenGroupRepository;
        }

        public ParkOpenGroupsModel CreateFromEntry (ParkOpenGroupsEntry entry)
        {
            var parkOpenGourps = entry.ParkOpenGroupIds.Select(id => {
                var parkOpenGroup = this.parkOpenGroupRepository.Get(id);
                Debug.Assert(parkOpenGroup != null, "ParkOpenGroupが見つかりません" + id);
                return parkOpenGroup;
            });

            var type = ParkOpenGroupsType.None;
            if (Enum.TryParse (entry.Type, out ParkOpenGroupsType outType)) {
                type = outType;
            }

            return new ParkOpenGroupsModel (
                entry.Id,
                type,
                entry.BackgroundSpriteName,
                parkOpenGourps);
        }

        public IEnumerable<ParkOpenGroupsModel> GetAll () {
            return entrys.Select (entry => {
                return Get(entry.Id);
            });
        }

        public ParkOpenGroupsModel Get (uint id) 
        {
            var entry = base.GetEntry(id);
            Debug.Assert(entry != null, id + "が見つかりません");
            return this.CreateFromEntry(entry);
        }

        public ParkOpenGroupsModel GetByType (ParkOpenGroupsType type)
        {
            var selectedEntrys = entrys.Where(entry => {
                return entry.Type == type.ToString();
            });
            Debug.Assert(selectedEntrys.Count() <= 1, "当てはまる情報が２つ以上あります");
            return this.CreateFromEntry(selectedEntrys.First());
        }

    }
}