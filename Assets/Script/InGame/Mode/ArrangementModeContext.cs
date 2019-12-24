using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {
    public class ArrangementModeContext {

        public IPlayerArrangementTargetRepository PlayerArrangementTargetRepository { get; private set;}

        public ArrangementModeContext(IPlayerArrangementTargetRepository playerArrangementTargetRepository)
        {
            this.PlayerArrangementTargetRepository = playerArrangementTargetRepository;            
        }

        // Mono
        private MonoInfo targetMonoInfo;
        public MonoInfo TargetMonoInfo => targetMonoInfo;

        public void SetTagetMonoInfo (MonoInfo monoInfo) {
            this.targetMonoInfo = monoInfo;
        }

        // Mouse
        private Mouse targetMouse;
        public Mouse TargetMouse => targetMouse;

        public void SetTagetMouse (Mouse targetMouse) {
            this.targetMouse = targetMouse;
        }
    }
}