using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public enum AppearCharacterState 
    {
        None,
        GoMono,
        PlayingMono,
        GoAway,
        Removed
    }

    public enum AppearCharacterLifeDirectorType
    {
        None,
        Reserve,
        ParkOpen
    }

    public class PlayerAppearCharacterViewModel : ModelBase
    {
        public AppearCharacterModel AppearCharacterModel { get; private set; }
        public Vector3 Position { get; private set; }
        public Vector3 Rotation { get; private set; }
        public PlayerAppearCharacterReserveModel PlayerAppearCharacterReserveModelInDirector { get; private set; }
        public bool IsReceiveReward { get; private set; }
        public AppearCharacterState AppearCharacterState { get; private set; }
        public PlayerArrangementTargetModel PlayerArrangementTargetModel { get; private set; }
        public float CurrentPlayingTime { get; private set; }
        public AppearCharacterLifeDirectorType AppearCharacterLifeDirectorType { get; private set; }
        public MovePath MovePath { get; private set; }

        public void ToReceiveRewards() {
            this.IsReceiveReward = true;
        }

        public void ChangeState(AppearCharacterState appearCharacterState) {
            this.AppearCharacterState = appearCharacterState;
        }

        public void UpdateTransform (Vector3 position, Vector3 rotation) {
            this.Position = position;
            this.Rotation = rotation;
        } 

        public void SetTargetArrangement(PlayerArrangementTargetModel playerArrangementTargetModel) {
            this.PlayerArrangementTargetModel = playerArrangementTargetModel;
        }

        public void SetCurrentPlayingTime (float currentPlayingTime) {
            this.CurrentPlayingTime = currentPlayingTime;
        }

        public PlayerAppearCharacterViewModel(
            uint id,
            AppearCharacterModel appearCharacterModel,
            Vector3 position, 
            Vector3 rotation, 
            PlayerAppearCharacterReserveModel playerAppearCharacterReserveModel, 
            bool isReceiveReward,
            AppearCharacterState appearCharacterState,
            PlayerArrangementTargetModel playerArrangementTargetModel,
            float currentPlayingTime,
            AppearCharacterLifeDirectorType appearCharacterLifeDirectorType,
            MovePath movePath)
        {
            this.Id = id;
            this.AppearCharacterModel = appearCharacterModel;
            this.Position = position;
            this.Rotation = rotation;
            this.PlayerAppearCharacterReserveModelInDirector = playerAppearCharacterReserveModel;
            this.IsReceiveReward = isReceiveReward;
            this.AppearCharacterState = appearCharacterState;
            this.PlayerArrangementTargetModel = playerArrangementTargetModel;
            this.CurrentPlayingTime = currentPlayingTime;
            this.AppearCharacterLifeDirectorType = appearCharacterLifeDirectorType;
            this.MovePath = movePath;
        }

    }   
}
