using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

namespace NL {
    [DataContract]
    public class MousePurchaceTableEntry : EntryBase {

        [DataMember]
        public long CurrencyCost { get; set; }

        [DataMember]
        public long ItemCost { get; set; }
    }

    public interface IMousePurchaceTableRepository {
        IEnumerable<MousePurchaceTableModel> GetAll ();
        MousePurchaceTableModel Get (uint id);
        uint MacPurchaseCount ();
    }

    public class MousePurchaceTableRepository : RepositoryBase<MousePurchaceTableEntry>, IMousePurchaceTableRepository {
        public MousePurchaceTableRepository (ContextMap contextMap) : base (contextMap.MousePurchaceTableEntrys) { }

        public IEnumerable<MousePurchaceTableModel> GetAll () {
            return entrys.Select (entry => {
                return new MousePurchaceTableModel (
                    entry.Id,
                    entry.CurrencyCost,
                    entry.ItemCost);
            });
        }

        public MousePurchaceTableModel Get (uint id) 
        {
            var entry = base.GetEntry(id);
            Debug.Assert(entry != null, id + "が見つかりません");
            return new MousePurchaceTableModel (
                    entry.Id,
                    entry.CurrencyCost,
                    entry.ItemCost);
        }

        public uint MacPurchaseCount () 
        {
            var maxId = this.entrys.Max(entry => entry.Id);
            return maxId;
        }
    }
}