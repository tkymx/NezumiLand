using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class EmptyState : IState
    {
        public void onEnter()
        {

        }

        public IState onUpdate()
        {
            return null;
        }

        public void onExit()
        {

        }
    }
}
