using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace NL {
    public class RepositoryBase<T> 
        where T : EntryBase {
        protected IList<T> entrys;

        protected RepositoryBase (IList<T> entrys) {
            this.entrys = entrys;
        }

        protected T GetEntry(uint id) {
            var result = this.entrys.Where (e => e.Id == id);
            Debug.Assert (result != null, "ファイルが見つかりません : " + id.ToString ());
            var entry = result.First();
            return entry;            
        }
    }
}