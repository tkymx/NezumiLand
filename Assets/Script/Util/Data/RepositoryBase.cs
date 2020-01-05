using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace NL {
    public class RepositoryBase<T> 
        where T : EntryBase {
        protected List<T> entrys;

        protected RepositoryBase (List<T> entrys) {
            this.entrys = entrys;
        }

        protected T GetEntry(uint id) {
            var entry = this.entrys.Find (e => e.Id == id);
            Debug.Assert (entry != null, "ファイルが見つかりません : " + id.ToString ());
            return entry;
        }
    }
}