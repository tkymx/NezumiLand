using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

namespace NL {

    [System.Serializable]
    public class ParkOpenPositionEntry : EntryBase {
        public Position3Entry Position;
        public string PositionType;
    }

    public interface IParkOpenPositionRepository {
        IEnumerable<ParkOpenPositionModel> GetAll ();
        ParkOpenPositionModel Get (uint id);
        ParkOpenPositionModel GerRandomPosition (ParkOpenPositionModel.PositionType type);
    }

    public class ParkOpenPositionRepository : RepositoryBase<ParkOpenPositionEntry>, IParkOpenPositionRepository {
        public ParkOpenPositionRepository (ContextMap contextMap) : base (contextMap.ParkOpenPositionEntrys) { }

        public ParkOpenPositionModel CreateFromEntry (ParkOpenPositionEntry entry)
        {
            var type = ParkOpenPositionModel.PositionType.None;
            if (Enum.TryParse (entry.PositionType, out ParkOpenPositionModel.PositionType outType)) {
                type = outType;
            }

            return new ParkOpenPositionModel (
                entry.Id,
                new Vector3(entry.Position.X, entry.Position.Y, entry.Position.Z),
                type);            
        }

        public IEnumerable<ParkOpenPositionModel> GetAll () {
            return entrys.Select (entry => {
                return Get(entry.Id);
            });
        }

        public ParkOpenPositionModel Get (uint id) 
        {
            var entry = base.GetEntry(id);
            Debug.Assert(entry != null, id + "が見つかりません");
            return this.CreateFromEntry(entry);
        }

        public ParkOpenPositionModel GerRandomPosition (ParkOpenPositionModel.PositionType type)
        {
            var entryWithType = this.entrys.Where(entry => entry.PositionType == type.ToString()).ToArray();
            var selectIndex = UnityEngine.Random.Range(0, entryWithType.Length-1);
            return this.Get(entryWithType[selectIndex].Id);                
        }
    }
}