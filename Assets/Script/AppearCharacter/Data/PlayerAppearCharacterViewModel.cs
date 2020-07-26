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
        Conversation,
        Onegai,
        Playing
    }

    public class PlayerAppearCharacterViewModel : ModelBase
    {
        public AppearCharacterModel AppearCharacterModel { get; private set; }
        public AppearCharacterState AppearCharacterState { get; private set; }

        // state で使用
        public Vector3 Position { get; private set; }
        public Vector3 Rotation { get; private set; }
        public float CurrentPlayingTime { get; private set; }
        public MovePath MovePath { get; private set; }
        public PlayerArrangementTargetModel PlayerArrangementTargetModel { get; private set; }

        // director で使用
        public AppearCharacterLifeDirectorType AppearCharacterLifeDirectorType { get; private set; }
        public PlayerAppearCharacterDirectorModelBase PlayerAppearCharacterDirectorModelBase { get; private set; }

        public void ChangeState(AppearCharacterState appearCharacterState) {
            this.AppearCharacterState = appearCharacterState;
        }

        public void UpdateTransform (Vector3 position, Vector3 rotation) {
            this.Position = position;
            this.Rotation = rotation;
        } 

        public void SetCurrentPlayingTime (float currentPlayingTime) {
            this.CurrentPlayingTime = currentPlayingTime;
        }

        public void SetTargetArrangement(PlayerArrangementTargetModel playerArrangementTargetModel) {
            this.PlayerArrangementTargetModel = playerArrangementTargetModel;
        }

        public PlayerAppearCharacterViewModel(
            uint id,
            AppearCharacterModel appearCharacterModel,
            Vector3 position, 
            Vector3 rotation, 
            AppearCharacterState appearCharacterState,
            float currentPlayingTime,
            PlayerArrangementTargetModel playerArrangementTargetModel,
            AppearCharacterLifeDirectorType appearCharacterLifeDirectorType,
            PlayerAppearCharacterDirectorModelBase playerAppearCharacterDirectorModelBase,
            MovePath movePath)
        {
            this.Id = id;
            this.AppearCharacterModel = appearCharacterModel;
            this.Position = position;
            this.Rotation = rotation;
            this.AppearCharacterState = appearCharacterState;
            this.CurrentPlayingTime = currentPlayingTime;
            this.PlayerArrangementTargetModel = playerArrangementTargetModel;
            this.AppearCharacterLifeDirectorType = appearCharacterLifeDirectorType;
            this.PlayerAppearCharacterDirectorModelBase = playerAppearCharacterDirectorModelBase;
            this.MovePath = movePath;
        }

    }   
}
