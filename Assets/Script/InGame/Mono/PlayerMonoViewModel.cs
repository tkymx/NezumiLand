using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class PlayerMonoViewModel : ModelBase
    {
        public MonoInfo MonoInfo { get; private set; }
        public int Level { get; private set; }

        public PlayerMonoViewModel(
            uint id,
            MonoInfo monoInfo,
            int level
        )
        {
            this.Id = id;
            this.MonoInfo = monoInfo;
            this.Level = level;            
        }

        public void LevelUp() {
            this.Level++;
        }
    }    
}