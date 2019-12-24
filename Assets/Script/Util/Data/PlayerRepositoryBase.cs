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
            var entry = this.entrys.Find (e => e.Id == id);
            return entry;            
        }

        // 最も高いIDを取得
        protected uint MaximuId () {
            if (this.entrys.Count <= 0) {
                return 0;
            }
            return this.entrys.Max(entry => entry.Id);
        }
    }
}