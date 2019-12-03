using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public class OnegaiMediaterManager {

        private readonly NearOnegaiMediater nearOnegaiMediater = null;
        public NearOnegaiMediater NearOnegaiMediater => nearOnegaiMediater;

        public OnegaiMediaterManager (IPlayerOnegaiRepository playerOnegaiRepository) {
            this.nearOnegaiMediater = new NearOnegaiMediater(playerOnegaiRepository);            
        }

        public void ChacheOnegai(OnegaiModel onegaiModel) {
            this.nearOnegaiMediater.ChacheOnegai(onegaiModel);
        }
    }
}