using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace NL
{
    public enum OnegaiState
    {
        Lock,
        UnLock,
        Clear
    }

    public class PlayerOnegaiModel
    {
        public uint Id { get; private set; }
        public OnegaiModel OnegaiModel { get; private set; }
        public OnegaiState OnegaiState { get; private set; }

        public PlayerOnegaiModel(
            uint id,
            OnegaiModel onegaiModel,
            string onegaiState)
        {
            this.Id = id;
            this.OnegaiModel = onegaiModel;

            this.OnegaiState = OnegaiState.Lock;
            if (Enum.TryParse(onegaiState, out OnegaiState outOnegaiState))
            {
                this.OnegaiState = outOnegaiState;
            }
        }
    }
}
