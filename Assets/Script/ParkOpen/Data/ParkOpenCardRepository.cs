using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

namespace NL {

    [System.Serializable]
    public class ParkOpenCardEntry : EntryBase {
        public string Name;
        public string ImageName;
        public string Description;
        public uint ParkOpenCardActionId;
    }

    public interface IParkOpenCardRepository {
        IEnumerable<ParkOpenCardModel> GetAll ();
        ParkOpenCardModel Get (uint id);
    }

    public class ParkOpenCardRepository : RepositoryBase<ParkOpenCardEntry>, IParkOpenCardRepository {

        private readonly IParkOpenCardActionRepository parkOpenCardActionRepository = null;

        public ParkOpenCardRepository (IParkOpenCardActionRepository parkOpenCardActionRepository, ContextMap contextMap) : base (contextMap.ParkOpenCardEntrys) 
        {
            this.parkOpenCardActionRepository = parkOpenCardActionRepository;
        }

        public ParkOpenCardModel CreateFromEntry (ParkOpenCardEntry entry)
        {
            var parkOpenCardActionModel = this.parkOpenCardActionRepository.Get(entry.ParkOpenCardActionId);
            Debug.Assert(parkOpenCardActionModel != null, "parkOpenCardActionModelが存在しません");

            return new ParkOpenCardModel (
                entry.Id,
                entry.Name,
                entry.ImageName,
                entry.Description,
                parkOpenCardActionModel);
        }

        public IEnumerable<ParkOpenCardModel> GetAll () {
            return entrys.Select (entry => {
                return Get(entry.Id);
            });
        }

        public ParkOpenCardModel Get (uint id) 
        {
            var entry = base.GetEntry(id);
            Debug.Assert(entry != null, id + "が見つかりません");
            return this.CreateFromEntry(entry);
        }
    }
}