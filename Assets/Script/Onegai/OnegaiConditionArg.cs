using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public struct OnegaiConditionArg
    {
        private string[] args;
        public string[] Args => args;

        public OnegaiConditionArg(string value)
        {
            this.args = value.Split(',');
        }

        public override string ToString()
        {
            return string.Join(",",Args);
        }
    }
}
