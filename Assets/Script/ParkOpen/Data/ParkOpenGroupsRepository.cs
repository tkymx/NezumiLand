using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

namespace NL {

    [System.Serializable]
    public class ParkOpenGroupsEntry : EntryBase {
        public uint[] ParkOpenGroupIds;
    }

    public interface IParkOpenGroupsRepository {
        IEnumerable<ParkOpenGroupsModel> GetAll ();
        ParkOpenGroupsModel Get (uint id);
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

            return new ParkOpenGroupsModel (
                entry.Id,
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
    }
}