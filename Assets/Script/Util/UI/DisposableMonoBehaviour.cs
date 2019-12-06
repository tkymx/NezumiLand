using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace  NL
{
    public abstract class DisposableMonoBehaviour : MonoBehaviour
    {
        protected List<IDisposable> disposables = new List<IDisposable>();

        private void OnDestroy() {
            if (this.disposables != null) {
                foreach (var disposable in this.disposables)
                {
                    disposable.Dispose();                    
                }                
            }
        }
    }   
}