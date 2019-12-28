using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public enum MouseViewState {
        None,
        Move,
        Making,
        BackToHome
    }

    public class PlayerMouseViewModel : ModelBase
    {
        public Vector3 Position { get; private set; }
        public Vector3 Rotation { get; private set; }
        public MouseViewState State { get; private set; }
        public MakingAmount MakingAmount { get; private set; }
        public PlayerArrangementTargetModel PlayerArrangementTargetModel { get; private set; }

        public PlayerMouseViewModel(
            uint id,
            Vector3 position,
            Vector3 rotation,
            MouseViewState state,
            MakingAmount makingAmount,
            PlayerArrangementTargetModel playerArrangementTargetModel
        )
        {
            this.Id = id;
            this.Position = position;
            this.Rotation = rotation;
            this.State = state;
            this.MakingAmount = makingAmount;
            this.PlayerArrangementTargetModel = playerArrangementTargetModel;
        }

        public void UpdateTransform (Vector3 position, Vector3 rotation) {
            this.Position = position;
            this.Rotation = rotation;
        } 

        public void ChangeState (MouseViewState state) {
            this.State = state;
        }

        public void UpdateMakingAmount (MakingAmount makingAmount) {
            this.MakingAmount = makingAmount;
        }
    }   
}