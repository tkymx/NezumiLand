using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public abstract class SelectBase : MonoBehaviour
    {
        public abstract void OnOver(RaycastHit hit);
        public abstract void OnSelect(RaycastHit hit);
    }
}