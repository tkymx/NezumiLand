
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;

namespace NL {
    [System.Serializable]
    public class EntryBase
    {
        
        public uint Id;

        public override bool Equals (object obj) {
            if (obj == null || GetType () != obj.GetType ()) {
                return false;
            }
            var value = obj as EntryBase;
            return Id.Equals (value.Id);
        }

        public override int GetHashCode () {
            return Id.GetHashCode ();
        }
    }
}