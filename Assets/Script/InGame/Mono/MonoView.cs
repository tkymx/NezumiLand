using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public class MonoView : MonoBehaviour 
    { 
        public void SetPosition(Vector3 position)
        {
            this.transform.localPosition = position;
        }
    }
}