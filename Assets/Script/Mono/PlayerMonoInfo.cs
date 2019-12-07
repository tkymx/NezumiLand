using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  NL
{
    public class PlayerMonoInfo : ModelBase
    {
        public MonoInfo MonoInfo { get; private set; }
        public bool IsRelease { get; private set; }

        public PlayerMonoInfo(
            uint id,
            MonoInfo monoInfo,
            bool isRelease
        )
        {
            this.Id = id;
            this.MonoInfo = monoInfo;
            this.IsRelease = isRelease;            
        }

        public void ToRelease () {
            Debug.Assert(!this.IsRelease, this.MonoInfo.Id.ToString() + "はすでに開放されています");
            this.IsRelease = true;
        }
    }    
}