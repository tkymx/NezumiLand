using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public enum ParkOpenCardActionType
    {
        None,
        CountToHeart,
    }

    /// <summary>
    /// 
    /// </summary>
    public class ParkOpenCardActionModel : ModelBase
    {
        public ParkOpenCardActionType ParkOpenCardActionType { get; private set; }
        public string[] Args { get; private set; }

        public ParkOpenCardActionModel(
            uint id,
            ParkOpenCardActionType parkOpenCardActionType,
            string[] args
        )
        {
            this.Id = id;
            this.ParkOpenCardActionType = parkOpenCardActionType;
            this.Args = args;
        }
    }
}

