using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public class ModelBase
    {
        public uint Id { get; protected set; }

        public override bool Equals (object obj) {
            if (obj == null || GetType () != obj.GetType ()) {
                return false;
            }
            var value = obj as EventConditionModel;
            return Id.Equals (value.Id);
        }

        public override int GetHashCode () {
            return Id.GetHashCode ();
        }
    }
}