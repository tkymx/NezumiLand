using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

namespace NL {

    [System.Serializable]
    public class ParkOpenCardActionEntry : EntryBase {
        public string ParkOpenCardActionType;
        public string[] Args;
    }

    public interface IParkOpenCardActionRepository {
        IEnumerable<ParkOpenCardActionModel> GetAll ();
        ParkOpenCardActionModel Get (uint id);
    }

    public class ParkOpenCardActionRepository : RepositoryBase<ParkOpenCardActionEntry>, IParkOpenCardActionRepository {
        public ParkOpenCardActionRepository (ContextMap contextMap) : base (contextMap.ParkOpenCardActionEntrys) { }

        public ParkOpenCardActionModel CreateFromEntry (ParkOpenCardActionEntry entry)
        {
            var type = ParkOpenCardActionType.None;
            if (Enum.TryParse (entry.ParkOpenCardActionType, out ParkOpenCardActionType outType)) {
                type = outType;
            }

            return new ParkOpenCardActionModel (
                entry.Id,
                type,
                entry.Args);            
        }

        public IEnumerable<ParkOpenCardActionModel> GetAll () {
            return entrys.Select (entry => {
                return Get(entry.Id);
            });
        }

        public ParkOpenCardActionModel Get (uint id) 
        {
            var entry = base.GetEntry(id);
            Debug.Assert(entry != null, id + "が見つかりません");
            return this.CreateFromEntry(entry);
        }
    }
}