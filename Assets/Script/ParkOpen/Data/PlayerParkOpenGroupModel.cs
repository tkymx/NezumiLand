using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    /// <summary>
    /// 開放の一つのグループの状態を管理する
    /// </summary>
    public class PlayerParkOpenGroupModel : ModelBase
    {
        private const float SPECIAL_RATE = 0.1f;

        public enum OpenType {
            Normal,
            Special
        };

        public ParkOpenGroupModel ParkOpenGroupModel { get; private set; }
        public bool IsOpen { get; private set; }
        public bool IsClear { get; private set; }
        public OpenType CurrentOpenType { get; private set; }

        /// <summary>
        /// 特殊状態かどうか？
        /// </summary>
        public bool IsSpecial => this.CurrentOpenType == PlayerParkOpenGroupModel.OpenType.Special;

        public PlayerParkOpenGroupModel(
            uint id,
            ParkOpenGroupModel parkOpenGroupModel,
            bool isOpen,
            bool isClear,
            OpenType currentOpenType)
        {
            this.Id = id;
            this.ParkOpenGroupModel = parkOpenGroupModel;
            this.IsOpen = isOpen;
            this.IsClear = isClear;
            this.CurrentOpenType = currentOpenType;
        }

        public void ToClear() 
        {
            this.IsClear = true;
        }

        public void Lot() 
        {
            var rate = Random.Range(0.0f, 1.0f);
            this.CurrentOpenType = PlayerParkOpenGroupModel.OpenType.Normal;
            if (rate < SPECIAL_RATE) {
                this.CurrentOpenType = PlayerParkOpenGroupModel.OpenType.Special;
            }
        }

        public void ToOpen()
        {
            this.IsOpen = true;
        }
    }   
}