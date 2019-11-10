using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public struct ArrangementCount
    {
        private uint monoInfoId;
        public uint MonoInfoId => monoInfoId;

        private long count;
        public long Count => count;

        public ArrangementCount(uint monoInfoId, long count)
        {
            this.monoInfoId = monoInfoId;
            this.count = count;
        }

        public override string ToString()
        {
            return count.ToString();
        }
    }
}