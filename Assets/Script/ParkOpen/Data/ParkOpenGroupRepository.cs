using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

namespace NL {

    [System.Serializable]
    public class ParkOpenGroupEntry : EntryBase {
        public uint[] ParkOpenWaveIds;
    }

    public interface IParkOpenGroupRepository {
        IEnumerable<ParkOpenGroupModel> GetAll ();
        ParkOpenGroupModel Get (uint id);
    }

    public class ParkOpenGroupRepository : RepositoryBase<ParkOpenGroupEntry>, IParkOpenGroupRepository {

        private IParkOpenWaveRepository parkOpenWaveRepository;

        public ParkOpenGroupRepository (IParkOpenWaveRepository parkOpenWaveRepository, ContextMap contextMap) : base (contextMap.ParkOpenGroupEntrys) {
            this.parkOpenWaveRepository = parkOpenWaveRepository;
        }

        public ParkOpenGroupModel CreateFromEntry (ParkOpenGroupEntry entry)
        {
            var parkOpenWaveModels = entry.ParkOpenWaveIds.Select(id => {
                var parkOpenWaveModel = this.parkOpenWaveRepository.Get(id);
                Debug.Assert(parkOpenWaveModel != null, "parkOpenWaveModelがありません。 " + id.ToString());
                return parkOpenWaveModel;
            }); 

            return new ParkOpenGroupModel (
                entry.Id,
                parkOpenWaveModels.ToArray());
        }

        public IEnumerable<ParkOpenGroupModel> GetAll () {
            return entrys.Select (entry => {
                return Get(entry.Id);
            });
        }

        public ParkOpenGroupModel Get (uint id) 
        {
            var entry = base.GetEntry(id);
            Debug.Assert(entry != null, id + "が見つかりません");
            return this.CreateFromEntry(entry);
        }
    }
}