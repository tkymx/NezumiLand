using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    namespace ParkOpenCardAction
    {
        /// <summary>
        /// Empty
        /// </summary>
        public class Empty : ParkOpenCardAction.IParkOpenCardAction
        {
            public ParkOpenCardModel ParkOpenCardModel => null;

            public void OnStart(ParkOpenCardModel parkOpenCardModel)
            {
            }
            public void OnUpdate()
            {
            }
            public bool IsAlive()
            {
                return false;
            }
            public void OnComplate()
            {
            }
        }
    }
}

