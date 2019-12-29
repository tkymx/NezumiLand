using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class PlayerAppearCharacterViewModel : ModelBase
    {
        public Vector3 Position { get; private set; }
        public Vector3 Rotation { get; private set; }
        public PlayerAppearCharacterReserveModel PlayerAppearCharacterReserveModel { get; private set; }
        public bool IsReceiveReward { get; private set; }

        public void ToReceiveRewards() {
            this.IsReceiveReward = true;
        }

        public PlayerAppearCharacterViewModel(
            uint id,
            Vector3 position, 
            Vector3 rotation, 
            PlayerAppearCharacterReserveModel playerAppearCharacterReserveModel, 
            bool isReceiveReward)
        {
            this.Id = id;
            this.Position = position;
            this.Rotation = rotation;
            this.PlayerAppearCharacterReserveModel = playerAppearCharacterReserveModel;
            this.IsReceiveReward = isReceiveReward;
        }

    }   
}
