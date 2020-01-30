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
    }

    public interface IParkOpenCardRepository {
        IEnumerable<ParkOpenCardModel> GetAll ();
        ParkOpenCardModel Get (uint id);
    }

    public class ParkOpenCardRepository : RepositoryBase<ParkOpenCardEntry>, IParkOpenCardRepository {
        public ParkOpenCardRepository (ContextMap contextMap) : base (contextMap.ParkOpenCardEntrys) { }

        public ParkOpenCardModel CreateFromEntry (ParkOpenCardEntry entry)
        {
            return new ParkOpenCardModel (
                entry.Id,
                entry.Name,
                entry.ImageName,
                entry.Description);            
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