using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    /*
        - uint Id
        - Vector3 centerPosition
        - float range
        - List<ArrangementPosition> position
        - ArrangementTargetState state
        - MonoInfo monoInfo
        - PlayerMonoViewModel playerMonoViewModel    
    */

    public class PlayerArrangementTargetModel : ModelBase
    {
        public Vector3 CenterPosition { get; private set; }
        public float Range { get; private set; }
        public List<ArrangementPosition> Positions { get; private set; }
        public ArrangementTargetState State { get; private set; }
        public MonoInfo MonoInfo { get; private set; }
        public PlayerMonoViewModel PlayerMonoViewModel { get; private set; }

        public PlayerArrangementTargetModel(
            uint id,
            Vector3 centerPosition,
            float range,
            List<ArrangementPosition> positions,
            ArrangementTargetState state,
            MonoInfo monoInfo,
            PlayerMonoViewModel playerMonoViewModel
        )
        {
            this.Id = id;
            this.CenterPosition = centerPosition;
            this.Range = range;
            this.Positions = positions;
            this.State = state;
            this.MonoInfo = monoInfo;
            this.PlayerMonoViewModel = playerMonoViewModel;
        }

        public void SetMonoInfo(MonoInfo monoInfo) {
            this.MonoInfo = monoInfo;
        }

        public void SetPlayerMonoView(PlayerMonoViewModel playerMonoViewModel) {
            this.PlayerMonoViewModel = playerMonoViewModel;
        }        

        public void ToAppear() {
            Debug.Assert(this.State != ArrangementTargetState.Appear, "すでにAppearです");
            this.State = ArrangementTargetState.Appear;
        }
    }   
}
