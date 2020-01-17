using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  NL
{
    public class RemovedState : IState
    {
        public RemovedState () {
        }

        public void onEnter () {

        }

        private bool isAlivable () {
            return true;
        }

        public IState onUpdate () {
            return null;
        }

        public void onExit () {

        }
    }    
}
