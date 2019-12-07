using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NL {
    public class PlayerRepositoryBase<T> 
        where T : EntryBase
    {
        protected List<T> entrys;

        protected PlayerRepositoryBase (IList<T> entrys) {
            this.entrys = entrys.ToList ();
        }

        // プレイヤーデータは始めない可能性もあるのでnull 許容
        protected T GetEntry(uint id) {
            var result = this.entrys.Where (e => e.Id == id);
            if (result.Count() <= 0) {
                return null;
            }
            var entry = result.First();
            return entry;            
        }
    }
}