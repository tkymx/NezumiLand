using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class ArrangementInfoGenerator
    {
        static public ArrangementInfo Generate(Vector3 position, MonoInfo mono)
        {
            var arrangementX = Mathf.FloorToInt(position.x / ArrangementAnnotater.ArrangementWidth);
            var arrangementZ = Mathf.FloorToInt(position.z / ArrangementAnnotater.ArrangementHeight);

            var info = new ArrangementInfo()
            {
                x = arrangementX,
                z = arrangementZ,
                mono = mono
            };

            return info;
        }
    }
}